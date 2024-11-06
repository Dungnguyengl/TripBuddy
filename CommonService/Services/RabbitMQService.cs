using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;

namespace CommonService.Services
{
    public class RabbitMQService(IConfiguration configuration)
    {
        private readonly string _host = configuration.GetValue<string>("RABBITMQ_HOST") ?? "localhost";
        private readonly int _port = configuration.GetValue<int?>("RABBITMQ_PORT") ?? 5672;
        private readonly ConcurrentDictionary<string, string> _consumers = [];
        private ConnectionFactory? _factory;
        private IConnection? _connection;
        private IModel _channel;

        public IConnection Connection
        {
            get
            {
                _factory ??= new() { HostName = _host, Port = _port };
                return _connection ??= _factory.CreateConnection();
            }
        }

        public IModel Channel
        {
            get
            {
                _channel ??= Connection.CreateModel();
                return _channel;
            }
        }

        public void Register(string queueName, EventHandler<BasicDeliverEventArgs> eventing)
        {
            Channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            var consumer = new EventingBasicConsumer(Channel);
            consumer.Received += eventing;
            var consumerTag = Channel.BasicConsume(consumer, queueName, autoAck: false);
            _consumers.TryAdd(queueName, consumerTag);
        }

        public void Unregister(string queueName)
        {
            var consumerTag = _consumers.GetValueOrDefault(queueName);
            if (consumerTag != null)
            {
                Channel.BasicCancel(consumerTag);
            }
        }
    }
}
