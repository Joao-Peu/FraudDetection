using FraudDetection.Models;
using FraudDetection.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace FraudDetection.Messaging
{
    public class TransactionConsumer : IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly FraudDetectionService _fraudDetectionService;

        public TransactionConsumer(IConfiguration configuration, FraudDetectionService fraudDetectionService)
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

            _fraudDetectionService = fraudDetectionService;
        }

        public void StartConsuming()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Received message: {message}");

                var transactionDTO = MapMessageToTransaction(message);

                // Verifique se a transação é fraudulenta usando os serviços de detecção
                var fraudResult = _fraudDetectionService.IsFraudulentTransaction(transactionDTO);

                if (fraudResult.Contains("risco de fraude") ||
                    _fraudDetectionService.CheckTransactionFrequency(transactionDTO) ||
                    _fraudDetectionService.CheckTransactionAmount(transactionDTO) ||
                    _fraudDetectionService.CheckTransactionLocation(transactionDTO))
                {
                    Console.WriteLine("Fraudulent transaction detected!");
                }
                else
                {
                    Console.WriteLine("Transaction is not fraudulent");
                }
            };

            _channel.BasicConsume(
                queue: "fraud-transactions",
                autoAck: true,
                consumer: consumer);
        }

        private TransactionDTO MapMessageToTransaction(string message)
        {
            return JsonSerializer.Deserialize<TransactionDTO>(message);
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}
