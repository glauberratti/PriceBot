using RabbitMQ.Client;

namespace PriceBot.Domain.Queue;

public interface IQueue
{
    IConnection ReturnRabbitMQConnection();
    IModel ReturnRabbitMQChannel(IConnection connection);
}
