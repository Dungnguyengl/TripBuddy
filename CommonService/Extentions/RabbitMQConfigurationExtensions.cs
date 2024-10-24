using CommonService.Configurations;
using Microsoft.Extensions.Hosting;

namespace CommonService.Extentions
{
    public static class RabbitMQConfigurationExtensions
    {
        public static IHostBuilder AddRabbitMQConfiguration(this IHostBuilder builder, string queueName)
        {
            return builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                var configBuild = config.Build();
                var section = configBuild.GetSection("ConfiguationService");
                var host = section.GetChildren().First(x => x.Key == "Host").Value;
                var port = section.GetChildren().First(x => x.Key == "Port").Value.ToIntNullable();
                config.Add(new RabbitMQConfigurationSource(queueName, host, port));
            });
        }
    }
}
