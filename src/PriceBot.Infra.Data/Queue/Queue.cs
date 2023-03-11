using Microsoft.Extensions.Options;
using PriceBot.CrossCutting.Settings;
using PriceBot.Domain.Queue;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace PriceBot.Infra.Data.Queue;

public class Queue : IQueue
{
    private readonly Settings _settings;

    public Queue(IOptions<Settings> settings)
	{
        _settings = settings.Value;
    }

    protected ConnectionFactory CreateConnectionFactory()
    {
        return new ConnectionFactory()
        {
            VirtualHost = _settings.RabbitMQConfig.VirtualHost,
            HostName = _settings.RabbitMQConfig.HostName,
            Port = _settings.RabbitMQConfig.Port,
            UserName = _settings.RabbitMQConfig.UserName,
            Password = _settings.RabbitMQConfig.Password
        };
    }

    private static void CreateQueue(IModel channel, string queueName)
    {
        channel.QueueDeclare(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false
        );
    }

    protected void PublishMessage<T>(string queueName, T message)
    {
        if (string.IsNullOrEmpty(queueName))
            throw new ArgumentNullException(nameof(queueName));
        if (message == null || message.Equals(default(T)))
            throw new ArgumentNullException(nameof(message));

        var jsonMessage = JsonSerializer.Serialize(message);
        var messageBody = Encoding.UTF8.GetBytes(jsonMessage);

        var factory = CreateConnectionFactory();

        try
        {
            // TODO: Log
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            CreateQueue(channel, queueName);
            var messageProperties = channel.CreateBasicProperties();
            messageProperties.Persistent = true;

            channel.BasicPublish(
                exchange: "",
                routingKey: queueName,
                basicProperties: messageProperties,
                body: messageBody
            );
        }
        catch (Exception)
        {
            // TODO: Log
            throw;
        }
    }

    protected T? GetMessage<T>(string queueName, bool removeFromQueue = false)
    {
        if (string.IsNullOrEmpty(queueName))
            throw new ArgumentNullException(nameof(queueName));

        var factory = CreateConnectionFactory();

        try
        {
            // TODO: Log
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            var data = channel.BasicGet(queueName, removeFromQueue);

            var body = data?.Body.ToArray();

            if (body is null)
            {
                return default;
            }

            var bodyStr = Encoding.UTF8.GetString(body);
            return JsonSerializer.Deserialize<T>(bodyStr);
        }
        catch (Exception)
        {
            // TODO: Log
            throw;
        }
    }

    public IConnection ReturnRabbitMQConnection()
    {
        var factory = CreateConnectionFactory();
        return factory.CreateConnection();
    }

    public IModel ReturnRabbitMQChannel(IConnection connection)
    {
        if (connection == null)
            throw new ArgumentNullException(nameof(connection));

        return connection.CreateModel();
    }
}
