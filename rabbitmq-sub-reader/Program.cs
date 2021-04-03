using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace rabbitmq_sub_reader
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string hostName = Environment.GetEnvironmentVariable("hostName");
            int port = int.Parse(Environment.GetEnvironmentVariable("port"));
            string queueName = Environment.GetEnvironmentVariable("queueName");

            ConnectionFactory factory = new ConnectionFactory() { HostName = hostName, Port = port };
            factory.AutomaticRecoveryEnabled = true;
            factory.UserName = "guest";
            factory.Password = "guest";
            IConnection conn = factory.CreateConnection();
            IModel channel = conn.CreateModel();
            channel.QueueDeclare(queue: queueName,
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(channel);

            channel.BasicConsume(queue: queueName,
                autoAck: false, consumer: consumer);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                String test = Encoding.UTF8.GetString(body);
                dynamic msg = JsonConvert.DeserializeObject(test);
                int sleep = msg.SleepTime;
                int sleepMs = sleep * 1000;
                Console.WriteLine(" [x] Received task: {0}, Length: {1}s, ", msg.Name, msg.SleepTime);
                Task.Delay(sleepMs).Wait();
                channel.BasicAck(
                    deliveryTag: ea.DeliveryTag,
                    multiple: false
                );
            };

            Console.WriteLine("Ready to read messages");
            await Task.Run(() => Thread.Sleep(Timeout.Infinite));
        }
    }
}
