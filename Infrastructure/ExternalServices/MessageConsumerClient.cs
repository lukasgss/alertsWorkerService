using System.Text;
using System.Text.Json;
using Application.Common.Entities;
using Application.Common.Interfaces;
using Infrastructure.ExternalServices.Configs;
using Infrastructure.ExternalServices.Interfaces;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infrastructure.ExternalServices;

public class MessageConsumerClient : IMessageConsumerClient
{
	private readonly IMessagingConnectionEstablisher _messagingConnectionEstablisher;
	private readonly RabbitMqData _rabbitMqData;

	public MessageConsumerClient(
		IMessagingConnectionEstablisher messagingConnectionEstablisher,
		IOptions<RabbitMqData> rabbitMqData)
	{
		_messagingConnectionEstablisher = messagingConnectionEstablisher ??
		                                  throw new ArgumentNullException(nameof(messagingConnectionEstablisher));
		_rabbitMqData = rabbitMqData.Value ?? throw new ArgumentNullException(nameof(rabbitMqData));
	}
	
	public void ConsumeFoundAlertMessages()
	{
		using IConnection connection = _messagingConnectionEstablisher.EstablishConnection();
		using IModel channel = connection.CreateModel();

		SetupRouting(channel);
		channel.BasicQos(prefetchSize: 0, prefetchCount: _rabbitMqData.PrefetchCount, global: false);

		AsyncEventingBasicConsumer consumer = new(channel);
		consumer.Received += async (_, eventArgs) =>
		{
			byte[] body = eventArgs.Body.ToArray();
			string messageString = Encoding.UTF8.GetString(body);
			FoundAnimalAlertMessage? messageData = JsonSerializer.Deserialize<FoundAnimalAlertMessage>(messageString);
			
			await Task.Yield();
		};

		channel.BasicConsume(_rabbitMqData.FoundAnimalsQueueName, autoAck: true, consumer);
	}

	private void SetupRouting(IModel channel)
	{
		channel.ExchangeDeclare(_rabbitMqData.AlertsExchangeName, ExchangeType.Topic, durable: true, autoDelete: false);
		channel.QueueDeclare(queue: _rabbitMqData.FoundAnimalsQueueName,
			durable: true,
			exclusive: false,
			autoDelete: false,
			arguments: null);
		channel.QueueBind(_rabbitMqData.FoundAnimalsQueueName,
			_rabbitMqData.AlertsExchangeName,
			_rabbitMqData.FoundAnimalsRoutingKey);
	}
}