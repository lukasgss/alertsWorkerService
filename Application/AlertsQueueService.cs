using Application.Common.Interfaces.ExternalServices;
using Application.Common.Interfaces.Services;

namespace Application;

public class AlertsQueueService : IAlertsQueueService
{
	private readonly IMessageConsumerClient _messageConsumerClient;

	public AlertsQueueService(IMessageConsumerClient messageConsumerClient)
	{
		_messageConsumerClient =
			messageConsumerClient ?? throw new ArgumentNullException(nameof(messageConsumerClient));
	}

	public void ConsumeAlerts()
	{
		// TODO: Consume other alert messages
		_messageConsumerClient.ConsumeFoundAlertMessages();
	}
}