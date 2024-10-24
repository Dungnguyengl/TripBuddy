using Microsoft.Extensions.Configuration;

namespace CommonService.Configurations
{
    public class RabbitMQConfigurationSource(string queueName, string? hostname = null, int? port = null) : IConfigurationSource
    {
        private readonly string _queueName = queueName;
        private readonly string? _hostName = hostname;
        private readonly int? _port = port;

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new RabbitMQConfigurationProvider(_queueName, _hostName, _port);
        }
    }
}
