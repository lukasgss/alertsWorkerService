namespace Application.Common.Interfaces.ExternalServices;

public interface IMessageConsumerClient
{
	public void ConsumeFoundAlertMessages();
}