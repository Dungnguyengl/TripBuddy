using CommonService.Extentions;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json.Nodes;

namespace CommonService.Configurations
{
    public class RabbitMQConfigurationProvider(string queueName, string? hostname, int? port) : ConfigurationProvider
    {
        private readonly string _queueName = queueName;
        private readonly string _hostName = hostname ?? "localhost";
        private readonly int _port = port ?? 5672;

        public override void Load()
        {
            LoadAsync().GetAwaiter().GetResult();
        }

        private async Task LoadAsync()
        {
            var factory = new ConnectionFactory() { HostName = _hostName, Port = _port };
            var tcs = new TaskCompletionSource<Dictionary<string, string>>();
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var jsonObject = JsonNode.Parse(message)?.AsObject();
                ArgumentNullException.ThrowIfNull(jsonObject, $"{nameof(RabbitMQConfigurationProvider)}-{nameof(LoadAsync)}");
                var configData = jsonObject.FlattenJson();
                tcs.SetResult(configData);
                foreach (var kvp in configData)
                {
                    Data[kvp.Key] = kvp.Value;
                }
            };
            channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);

            var config = await tcs.Task;
            Data.Clear();
            foreach (var kvp in config)
            {
                Data[kvp.Key] = kvp.Value;
            }
        }
    }
}
