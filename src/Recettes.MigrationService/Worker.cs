
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

using OpenTelemetry.Trace;
using Recettes.Data.Contexts;
using Recettes.Data.Models;
using System.Diagnostics;

namespace Recettes.MigrationService;

public class Worker(
    IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
    public const string ActivitySourceName = "Migrations";
    private static readonly ActivitySource s_activitySource = new(ActivitySourceName);


    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var activity = s_activitySource.StartActivity("Migrating database", ActivityKind.Client);

        try
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<RecettesContext>();

            await EnsureDatabaseAsync(dbContext, cancellationToken);
            await RunMigrationAsync(dbContext, cancellationToken);
            await SeedDataAsync(dbContext, cancellationToken);
        }
        catch (Exception ex)
        {
            activity?.RecordException(ex);
            throw;
        }

        hostApplicationLifetime.StopApplication();
    }
    private static async Task EnsureDatabaseAsync(RecettesContext dbContext, CancellationToken cancellationToken)
    {
        var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Create the database if it does not exist.
            // Do this first so there is then a database to start a transaction against.
            if (!await dbCreator.ExistsAsync(cancellationToken))
            {
                await dbCreator.CreateAsync(cancellationToken);
            }
        });
    }

    private static async Task RunMigrationAsync(RecettesContext dbContext, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Run migration in a transaction to avoid partial migration if it fails.
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            await dbContext.Database.MigrateAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        });
    }

    private static async Task SeedDataAsync(RecettesContext dbContext, CancellationToken cancellationToken)
    {
        Recipe recipe = new()
        {
            Name = "Test Recipe",
            Description = "Default recipe, please ignore!",
            Ingredients = [
                new() { Name = "Chicken",  CreatedAt = DateTime.UtcNow,UpdatedAt = DateTime.UtcNow,Id=1},
                new() { Name = "Pepperoni" , CreatedAt = DateTime.UtcNow,UpdatedAt = DateTime.UtcNow,Id=2}],
            Instructions = [new() { Description = "Cut Chicken", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }, new() { Description = "Taste the pepperoni", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }],
            ImageUrl = "https://via.placeholder.com/150",
            Servings = 4,
            PrepTime = 10,
            CookTime = 20,
            TotalTime = 30,
            Source = "Test source",
            Url = "https://via.placeholder.com/150",
            Notes = "Test notes",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Seed the database
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            await dbContext.Recipes.AddAsync(recipe, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        });
    }
}
