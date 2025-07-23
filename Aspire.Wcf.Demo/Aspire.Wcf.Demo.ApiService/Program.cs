using Aspire.Wcf.Demo.ApiService;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<DingClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.MapPost("/ding", ([FromBody] DingRequest request, [FromServices] DingClient dingClient) =>
	{
		var ding = dingClient.CallDing(request.Name);
		return Results.Ok(new DingResponse(ding));
	})
	.WithName("Ding")
	.Produces<DingResponse>()
	.WithOpenApi();

app.MapDefaultEndpoints();

app.Run();

internal record DingRequest(string Name);

internal record DingResponse(string Message);