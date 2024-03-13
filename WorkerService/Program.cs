using Application;
using Infrastructure;
using WorkerService;
using WorkerService.Extensions;
using WorkerService.Interfaces;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.ConfigureIdentity();

builder.Services
	.AddApplication()
	.AddInfrastructure(builder.Configuration);

builder.Services.AddHostedService<Worker>();
builder.Services.AddScoped<IScopedProcessingService, ScopedProcessingService>();

var host = builder.Build();
host.Run();