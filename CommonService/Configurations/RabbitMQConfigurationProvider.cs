using CommonService.Service;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace CommonService.Configurations
{
    public class RabbitMQConfigurationProvider(string queueName, string? hostname, int? port) : ConfigurationProvider
    {
        private readonly string _queueName = queueName;
        private readonly string _hostName = hostname ?? "localhost";
        private readonly int _port = port ?? 5672;
        private readonly JsonFileService _fileService = new();
        private ConnectionFactory _factory;

        private IConnection? _connection;

        public IConnection Connection
        {
            get
            {
                _factory ??= new() { HostName = _hostName, Port = _port };
                return _connection ??= _factory.CreateConnection();
            }
        }

        public override void Load()
        {
            LoadAsync();
        }

        private void LoadAsync()
        {
            var channel = Connection.CreateModel();
            channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _fileService.SaveConfigurations(message);
                //var jsonObject = JsonNode.Parse(message)?.AsObject();
                //ArgumentNullException.ThrowIfNull(jsonObject, $"{nameof(RabbitMQConfigurationProvider)}-{nameof(LoadAsync)}");
                //var configData = jsonObject.FlattenJson();
                //Data.Clear();
                //foreach (var kvp in configData)
                //{
                //    Data[kvp.Key] = kvp.Value;
                //}
                OnReload();
            };
            channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
        }
    }
}
