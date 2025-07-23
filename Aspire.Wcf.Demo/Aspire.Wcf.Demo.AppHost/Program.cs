using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var wcfService = builder.AddProject<Aspire_Wcf_Demo_LegacyService>("wcfservice");

builder.AddProject<Aspire_Wcf_Demo_ApiService>("apiservice")
	.WithHttpsHealthCheck("/health")
	.WithReference(wcfService);

builder.Build().Run();