var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.HelloAspire_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.HelloAspire_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.AddPnpmApp("reactapp", "../ReactRouterApp", scriptName: "dev")
    .WithPnpmPackageInstallation()
    .WithHttpEndpoint(name: "http", port: 5173, env: "VITE_PORT", isProxied: false)
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();
// .WithHttpCommand("dev")
// .WithEnvironment("NODE_ENV", "development");

builder.Build().Run();
