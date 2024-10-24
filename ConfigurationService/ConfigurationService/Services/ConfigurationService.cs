using CommonService.Extentions;
using RabbitMQ.Client;
using System.Text.Json.Nodes;

namespace ConfigurationService.Services
{
    public class ConfigurationService
    {
        private readonly string _filePath = "configurations.json";
        private readonly ConnectionFactory _factory;

        public ConfigurationService(IConfiguration cfg)
        {
            var host = cfg.GetValue<string>("RABBITMQ_HOST") ?? "localhost";
            var port = cfg.GetValue<int?>("RABBITMQ_PORT") ?? 5672;
            _factory = new ConnectionFactory() { HostName = host, Port = port };
            PushAllSectionToRabbitMQ();
        }

        public Dictionary<string, string> LoadConfigurations()
        {
            if (!File.Exists(_filePath))
            {
                throw new ArgumentNullException(nameof(LoadConfigurations));
            }

            var json = File.ReadAllText(_filePath);
            var jsonNode = JsonNode.Parse(json).AsObject();
            return jsonNode.FlattenJson();
        }

        public void SaveConfigurations(Dictionary<string, string> config)
        {
            var raw = config.RevertToNestedJson();
            var json = raw.ToString();
            File.WriteAllText(_filePath, json);
        }

        public void PushConfigurationToRabbitMQ(string section, Dictionary<string, string> config)
        {
            using var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();

            var queueName = $"{section}-config-queue";
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            // Purge previous messages
            channel.QueuePurge(queueName);

            var raw = config.ReduceLevels()
                .RevertToNestedJson()
                .ToString();
            var body = raw.ConvertStringToByteArray();
            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
        }

        public void PushAllSectionToRabbitMQ()
        {
            var config = LoadConfigurations();
            var sections = config.Keys.Select(key => key.Split(':').FirstOrDefault())
                .Where(key => !key.IsNullOrEmpty() && key != "global")
                .Distinct();

            using var connection = RetryExtentions.Retry(() => _factory.CreateConnection(), 10, 3000);
            using var channel = RetryExtentions.Retry(() => connection.CreateModel());

            foreach (var section in sections)
            {
                var queueName = $"{section}-config-queue";
                channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                // Purge previous messages
                channel.QueuePurge(queueName);

                var raw = config.Where(kvp => kvp.Key.StartsWith(section) || kvp.Key.StartsWith("global"))
                    .ToDictionary()
                    .ReduceLevels()
                    .RevertToNestedJson()
                    .ToString();
                var body = raw.ConvertStringToByteArray();
                channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
            }
        }
    }
}
