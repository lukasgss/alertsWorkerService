using Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddSingleton<IAlertsQueueService, AlertsQueueService>();
		
		return services;
	}
}