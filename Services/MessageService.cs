using System;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace DotNetToMQService.Services
{
    public class MessageService : IMessageService
    {
        private readonly IConnectionFactory _connectionFactory;

        public MessageService(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public object GenerateMessage()
        {
            // Step 1: Generate the random message data
            var message = new
            {
                Id = Guid.NewGuid(),
                Name = "RandomName-" + new Random().Next(1, 1000),
                Timestamp = DateTime.UtcNow.ToString("o")
            };

            // Step 2: Serialize the message data to JSON
            var jsonMessage = JsonSerializer.Serialize(message);

            // Step 3: Send the message to RabbitMQ
            using var connection = _connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            // Declare a queue in RabbitMQ (idempotent)
            var queueName = "data-queue";
            channel.QueueDeclare(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            // Publish the message to the queue
            var body = Encoding.UTF8.GetBytes(jsonMessage);
            channel.BasicPublish(
                exchange: "",          // Default RabbitMQ exchange
                routingKey: queueName, // Routing key = queue name
                basicProperties: null,
                body: body);

            return message; // Return the message for debugging/logging purposes
        }
    }
}