using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace FraudDetection.Messaging
{
    public class TransactionConsumer
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public TransactionConsumer(IConfiguration configuration)
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

        public void StartConsuming()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Received message: {message}");

                //Adicionar logica para validar fraudes
            };

            _channel.BasicConsume(
                queue: "fraud-transactions",
                autoAck: true,
                consumer: consumer);
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}
