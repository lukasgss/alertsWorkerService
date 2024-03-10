using Application.Common.Interfaces;

namespace Application;

public class AlertsQueueService: IAlertsQueueService
{
	private readonly IMessageConsumerClient _messageConsumerClient;

	public AlertsQueueService(IMessageConsumerClient messageConsumerClient)
	{
		_messageConsumerClient = messageConsumerClient ?? throw new ArgumentNullException(nameof(messageConsumerClient));
	}

	public void ConsumeAlerts()
	{
		_messageConsumerClient.ConsumeFoundAlertMessages();
	}
}