var builder = DistributedApplication.CreateBuilder(args);

var wcfService = builder.AddProject<Projects.Aspire_Wcf_Demo_LegacyService>("wcfservice")
	.WithHttpEndpoint(name: "http", targetPort: 8080);

var apiService = builder.AddProject<Projects.Aspire_Wcf_Demo_ApiService>("apiservice")
	.WithHttpsHealthCheck("/health")
	.WithReference(wcfService);

builder.Build().Run();