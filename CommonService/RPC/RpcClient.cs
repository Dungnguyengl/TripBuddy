using CommonService.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;

namespace CommonService.RPC
{
    public class RpcClient : IDisposable
    {
        private const string EXCHARE_NAME = "rpc_excharge";

        private readonly RabbitMQService _rabbit;
        private readonly string replyQueueName;
        private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> callbackMapper = new();

        public RpcClient(RabbitMQService rabbit)
        {
            _rabbit = rabbit;
            _rabbit.Channel.ExchangeDeclare(EXCHARE_NAME, ExchangeType.Direct);
            replyQueueName = _rabbit.Channel.QueueDeclare().QueueName;
            _rabbit.Channel.QueueBind(replyQueueName, EXCHARE_NAME, replyQueueName);
            var consumer = new EventingBasicConsumer(_rabbit.Channel);
            consumer.Received += (model, ea) =>
            {
                if (!callbackMapper.TryRemove(ea.BasicProperties.CorrelationId, out var tcs))
                    return;
                var body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);
                tcs.TrySetResult(response);
            };

            _rabbit.Channel.BasicConsume(consumer: consumer,
                                         queue: replyQueueName,
                                         autoAck: true);
        }

        public Task<string> CallAsync(string message, string target, string type, IDictionary<string, object>? headers = null, CancellationToken cancellationToken = default)
        {
            IBasicProperties props = _rabbit.Channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;
            props.ReplyTo = replyQueueName;
            props.Type = type;
            props.Headers = headers;
            var messageBytes = Encoding.UTF8.GetBytes(message);
            var tcs = new TaskCompletionSource<string>();
            callbackMapper.TryAdd(correlationId, tcs);

            _rabbit.Channel.BasicPublish(exchange: EXCHARE_NAME,
                                 routingKey: target,
                                 basicProperties: props,
                                 body: messageBytes);

            cancellationToken.Register(() => callbackMapper.TryRemove(correlationId, out _));
            return tcs.Task;
        }

        public Task<string> CallAsync(byte[] messageBytes, string target, string type, IDictionary<string, object>? headers = null, CancellationToken cancellationToken = default)
        {
            IBasicProperties props = _rabbit.Channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;
            props.ReplyTo = replyQueueName;
            props.Type = type;
            props.Headers = headers;
            var tcs = new TaskCompletionSource<string>();
            callbackMapper.TryAdd(correlationId, tcs);

            _rabbit.Channel.BasicPublish(exchange: EXCHARE_NAME,
                                 routingKey: target,
                                 basicProperties: props,
                                 body: messageBytes);

            cancellationToken.Register(() => callbackMapper.TryRemove(correlationId, out _));
            return tcs.Task;
        }

        public void Dispose()
        {
            _rabbit.Connection.Close();
            GC.SuppressFinalize(this);
        }
    }
}
