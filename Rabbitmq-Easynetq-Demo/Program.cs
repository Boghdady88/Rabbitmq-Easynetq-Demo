using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Rabbitmq_Easynetq_Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.UserName = "ahmed";
            factory.Password = "P@ssw0rd";
            factory.HostName = "localhost";

            var conn = factory.CreateConnection();

            var channel = conn.CreateModel();
            channel.ExchangeDeclare("PaychoiceExchange", ExchangeType.Direct);

            channel.QueueDeclare("PaychoiceQueue", false, false, false, null);

            channel.QueueBind("PaychoiceQueue", "PaychoiceExchange", "routing key");

            for (int i = 0; i < 5; i++)
            {
                var message = new MyMessage
                {
                    Name = "ahmed",
                    Address = "Victoria",
                };
                var messageBodyStr = JsonConvert.SerializeObject(message);
                var messageBodyBytes = Encoding.UTF8.GetBytes(messageBodyStr);
                channel.BasicPublish("PaychoiceExchange", "routing key", null, messageBodyBytes);

                Console.WriteLine("Published message :" + message);
            }

            channel.Dispose();
            conn.Dispose();
        }
    }

    public class MyMessage
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
