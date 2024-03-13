using Application.Common.Interfaces.ExternalServices;
using Application.Common.Interfaces.Repositories;
using Infrastructure.ExternalServices;
using Infrastructure.ExternalServices.Configs;
using Infrastructure.ExternalServices.Interfaces;
using Infrastructure.Persistence.DataContext;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddScoped<IMessageConsumerClient, MessageConsumerClient>();
		services.AddScoped<IMessagingConnectionEstablisher, MessagingConnectionEstablisher>();
		services.AddScoped<IFoundAlertNotificationsRepository, FoundAlertNotificationsRepository>();

		services.Configure<RabbitMqData>(configuration.GetSection("RabbitMQ"));

		services.AddDbContext<AppDbContext>(options =>
			options.UseNpgsql(configuration.GetConnectionString("DefaultConnection") ?? string.Empty,
					o => o.UseNetTopologySuite())
				.UseEnumCheckConstraints());

		return services;
	}
}