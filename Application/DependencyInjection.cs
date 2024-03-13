using Application.Common.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddScoped<IAlertsQueueService, AlertsQueueService>();

		return services;
	}
}