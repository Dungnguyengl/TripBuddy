using CommonService.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace CommonService.Extentions
{
    public static class RabbitMQConfigurationExtensions
    {
        private static string _queueName;
        private static string _configFilePath = "configurations.json";

        public static IHostBuilder AddTripbuddyConfiguration(this IHostBuilder builder, string queueName)
        {
            _queueName = queueName;
            return builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                var configBuild = config.Build();
                var host = configBuild.GetValue<string>("RABBITMQ_HOST");
                var port = configBuild.GetValue<int?>("RABBITMQ_PORT") ?? 5672;
                if (!File.Exists(_configFilePath))
                {
                    File.WriteAllTextAsync(_configFilePath, "{}");
                }
                config.AddJsonFile(_configFilePath, false, true);
                config.Add(new RabbitMQConfigurationSource(queueName, host, port));
            });
        }
    }
}
