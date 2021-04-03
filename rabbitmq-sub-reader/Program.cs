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
            ConnectionFactory factory = new ConnectionFactory() { HostName = "host.docker.internal", Port = 5672, };
            factory.AutomaticRecoveryEnabled = true;
            factory.UserName = "guest";
            factory.Password = "guest";
            IConnection conn = factory.CreateConnection();
            IModel channel = conn.CreateModel();
            channel.QueueDeclare(queue: "task-queue",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(channel);

            channel.BasicConsume(queue: "task-queue",
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
