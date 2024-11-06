using CommonService.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace CommonService.RPC
{
    public abstract class RpcServiceBase
    {
        private const string EXCHARE_NAME = "rpc_excharge";

        private readonly RabbitMQService _rabbit;
        private readonly ILogger _logger;

        protected RpcServiceBase(string routing, RabbitMQService rabbit, ILogger logger)
        {
            _rabbit = rabbit;
            _logger = logger;
            _rabbit.Channel.ExchangeDeclare(EXCHARE_NAME, ExchangeType.Direct);
            var queueName = _rabbit.Channel.QueueDeclare().QueueName;
            _rabbit.Channel.QueueBind(queueName, EXCHARE_NAME, routing);
            var consumer = new EventingBasicConsumer(_rabbit.Channel);
            _rabbit.Channel.BasicConsume(queue: queueName,
                                         autoAck: false,
                                         consumer: consumer);

            consumer.Received += (model, ea) =>
            {
                StringBuilder response = new();

                var body = ea.Body.ToArray();
                var props = ea.BasicProperties;
                var replyProps = _rabbit.Channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;

                try
                {
                    var message = Encoding.UTF8.GetString(body);
                    var type = props.Type;
                    var headers = props.Headers;
                    response.Append(Handle(message, type, headers));
                    response.Append(Handle(body, type, headers));
                }
                catch (Exception e)
                {
                    response.Append($"{{ Message: \"{e.Message}\" }}");
                    _logger.LogError(e, "");
                }
                finally
                {
                    if (response.Length == 0)
                    {
                        response.Append($"{{ Message: \"Not handle for {props.Type} yet!\" }}");
                    }
                    var responseBytes = Encoding.UTF8.GetBytes(response.ToString());
                    _rabbit.Channel.BasicPublish(exchange: EXCHARE_NAME,
                                                 routingKey: props.ReplyTo,
                                                 basicProperties: replyProps,
                                                 body: responseBytes);
                    _rabbit.Channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
            };
        }

        protected abstract string? Handle(string message, string type, IDictionary<string, object> headers);
        protected abstract string? Handle(byte[] message, string type, IDictionary<string, object> headers);
    }
}
