using RabbitMQ.Client;
using System.Text;

public class RabbitMQService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMQService(IConfiguration configuration)
    {
        var factory = new ConnectionFactory
        {
            HostName = configuration["RabbitMQ:Host"]
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(
            queue: configuration["RabbitMQ:Queue"],
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }

    public void PublishMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(
            exchange: "",
            routingKey: "fraud-transactions",
            basicProperties: null,
            body: body);
    }

    public void Dispose()
    {
        _channel.Close();
        _connection.Close();
    }
}
