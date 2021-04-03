using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using rabbitmq_pub_api.Models;

namespace rabbitmq_pub_api.Services
{
    public class MessageSender : IMessageSender
    {
        ConnectionFactory _factory;
        IConnection _connection;
        IModel _channel;

        public MessageSender()
        {
            _factory = new ConnectionFactory() { HostName = "host.docker.internal", Port = 5672, };
            _factory.AutomaticRecoveryEnabled = true;
            _factory.UserName = "guest";
            _factory.Password = "guest";
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "task-queue",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
        }

        public void SendMessage(Task task)
        {
            string message = JsonConvert.SerializeObject(task);
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "",
                routingKey: "task-queue",
                basicProperties: null,
                body: body
            );

            Console.WriteLine($" [x] Published task: {task.Name} with id: {task.Id} to RabbitMQ");
        }
    }
}
