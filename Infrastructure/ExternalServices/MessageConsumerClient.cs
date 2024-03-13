using System.Text;
using System.Text.Json;
using Application.Common.Entities;
using Application.Common.Interfaces.ExternalServices;
using Application.Common.Interfaces.Repositories;
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
	private readonly IFoundAlertNotificationsRepository _foundAlertNotificationsRepository;

	public MessageConsumerClient(
		IMessagingConnectionEstablisher messagingConnectionEstablisher,
		IOptions<RabbitMqData> rabbitMqData,
		IFoundAlertNotificationsRepository foundAlertNotificationsRepository)
	{
		_messagingConnectionEstablisher = messagingConnectionEstablisher ??
		                                  throw new ArgumentNullException(nameof(messagingConnectionEstablisher));
		_rabbitMqData = rabbitMqData.Value ?? throw new ArgumentNullException(nameof(rabbitMqData));
		_foundAlertNotificationsRepository = foundAlertNotificationsRepository ??
		                                     throw new ArgumentNullException(nameof(foundAlertNotificationsRepository));
	}

	public void ConsumeFoundAlertMessages()
	{
		IConnection connection = _messagingConnectionEstablisher.EstablishConnection();
		IModel channel = connection.CreateModel();

		SetupRouting(channel);
		channel.BasicQos(prefetchSize: 0, prefetchCount: _rabbitMqData.PrefetchCount, global: false);

		EventingBasicConsumer consumer = new(channel);
		consumer.Received += (_, eventArgs) =>
		{
			byte[] body = eventArgs.Body.ToArray();
			string messageString = Encoding.UTF8.GetString(body);
			FoundAnimalAlertMessage? messageData = JsonSerializer.Deserialize<FoundAnimalAlertMessage>(messageString);
			if (messageData is null)
			{
				throw new Exception("Could not parse message data.");
			}

			var usersThatMatchPreferences =
				_foundAlertNotificationsRepository.GetUsersThatMatchPreferences(messageData);

			if (usersThatMatchPreferences.Any())
			{
				_foundAlertNotificationsRepository.SaveNotifications(
					usersThatMatchPreferences.Select(preferences => preferences.User).ToList(),
					messageData);
			}

			channel.BasicAck(deliveryTag: eventArgs.DeliveryTag, multiple: false);
			channel.Dispose();
		};

		channel.BasicConsume(_rabbitMqData.FoundAnimalsQueueName, autoAck: false, consumer);
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