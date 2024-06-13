using Recettes.Data.Contexts;
using Recettes.MigrationService;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.AddNpgsqlDbContext<RecettesContext>("postgresdb");

var host = builder.Build();
host.Run();
