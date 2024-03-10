using RabbitMQ.Client;

namespace Infrastructure.ExternalServices.Interfaces;

public interface IMessagingConnectionEstablisher
{
	IConnection EstablishConnection();
}