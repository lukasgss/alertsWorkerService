using Application;
using Infrastructure;
using WorkerService;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
	.AddApplication()
	.AddInfrastructure(builder.Configuration);

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
