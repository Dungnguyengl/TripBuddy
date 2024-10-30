using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace CommonService.Configurations
{
    public class RabbitMQService(IConfiguration configuration)
    {
        private readonly string _host = configuration.GetValue<string>("RABBITMQ_HOST") ?? "localhost";
        private readonly int _port = configuration.GetValue<int?>("RABBITMQ_PORT") ?? 5672;
        private ConnectionFactory? _factory;
        private IConnection? _connection;

        public IConnection Connection
        {
            get
            {
                _factory ??= new() { HostName = _host, Port = _port };
                return _connection ??= _factory.CreateConnection();
            }
        }

        public void OnReceived(string queueName, Action<string> onChange, bool isDeleteMessage = false)
        {
            var channel = Connection.CreateModel();
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (obj, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                onChange.Invoke(message);
            };
            channel.BasicConsume(consumer, queueName,autoAck: isDeleteMessage);
        }
    }
}
