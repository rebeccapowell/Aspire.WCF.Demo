using Aspire.Wcf.Demo.Contracts;

var builder = WebApplication.CreateBuilder();
builder.AddServiceDefaults();

builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();
builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

var app = builder.Build();

app.UseServiceModel(serviceBuilder =>
{
	serviceBuilder.AddService<DingService>();
	serviceBuilder.AddServiceEndpoint<DingService, IDingService>(new BasicHttpBinding(BasicHttpSecurityMode.Transport), "/DingService.svc");
	var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
	serviceMetadataBehavior.HttpsGetEnabled = true;
});

app.Run();