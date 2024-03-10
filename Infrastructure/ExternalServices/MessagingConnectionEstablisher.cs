using Infrastructure.ExternalServices.Configs;
using Infrastructure.ExternalServices.Interfaces;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Infrastructure.ExternalServices;

public class MessagingConnectionEstablisher : IMessagingConnectionEstablisher
{
	private readonly RabbitMqData _rabbitMqData;
	private readonly IConnection? _connection = null;

	public MessagingConnectionEstablisher(IOptions<RabbitMqData> rabbitMqData)
	{
		_rabbitMqData = rabbitMqData.Value ?? throw new ArgumentNullException(nameof(rabbitMqData));
	}

	public IConnection EstablishConnection()
	{
		if (_connection is not null)
		{
			return _connection;
		}

		ConnectionFactory factory = new()
		{
			HostName = _rabbitMqData.HostName,
			UserName = _rabbitMqData.Username,
			Password = _rabbitMqData.Password,
			Port = _rabbitMqData.Port,
		};

		return factory.CreateConnection();
	}
}