var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .AddDatabase("postgresdb");

var apiService = builder.AddProject<Projects.Recettes_ApiService>("apiservice")
                        .WithReference(postgres);

builder.AddProject<Projects.Recettes_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.AddProject<Projects.Recettes_MigrationService>("migrations")
    .WithReference(postgres);

builder.Build().Run();
