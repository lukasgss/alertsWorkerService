using Application.Common.Interfaces;
using Infrastructure.ExternalServices;
using Infrastructure.ExternalServices.Configs;
using Infrastructure.ExternalServices.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddSingleton<IMessageConsumerClient, MessageConsumerClient>();
		services.AddSingleton<IMessagingConnectionEstablisher, MessagingConnectionEstablisher>();
		
		services.Configure<RabbitMqData>(configuration.GetSection("RabbitMQ"));
		
		return services;
	}
}