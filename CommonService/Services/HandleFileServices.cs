using CommonService.Extentions;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.IO;
using System.Text;
using System.Threading.Channels;

namespace CommonService.Services
{
    public class FileProviderService(RabbitMQService rabbit, ILogger<FileProviderService> logger)
    {
        private readonly RabbitMQService _rabbit = rabbit;
        private readonly ILogger<FileProviderService> _logger = logger;

        public void SendFileAsync(Stream fileStream, string queueName = "file_chunks", string? correlationId = null, string? replyTo = null)
        {
            var channel = _rabbit.Channel;
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);
            IBasicProperties props = channel.CreateBasicProperties();
            props.CorrelationId = correlationId ?? Guid.NewGuid().ToString();
            props.ReplyTo = replyTo ?? string.Empty;
            byte [] buffer = new byte [100 * 1024]; // 100KB chunks
            int bytesRead;
            int chunkNumber = 0;

            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                var body = new byte [bytesRead];
                Array.Copy(buffer, body, bytesRead);

                props.Persistent = true;
                props.Headers = new Dictionary<string, object>
                    {
                        { "chunkNumber", chunkNumber },
                        { "totalBytes", fileStream.Length }
                    };
                RetryExtentions.Retry(() =>
                {
                    _logger.LogTrace("Start sent chunk {chunkNumber} of {correlationId}", chunkNumber, props.CorrelationId);
                    channel.BasicPublish(exchange: string.Empty, routingKey: queueName, basicProperties: props, body: body);
                });
                chunkNumber++;
            }
        }
    }

    public class FileReceiverService
    {
        private readonly RabbitMQService _rabbit;
        private readonly ILogger<FileReceiverService> _logger;
        private readonly Dictionary<string, (MemoryStream stream, long totalBytes, int chunksReceived)> _fileStream = [];
        private string _queueName = "file_chunks";

        public event Action<Stream, Guid> FileCompleted;

        public string QueueName
        {
            get => _queueName;
            set
            {
                if (_queueName != value)
                {
                    _rabbit.Unregister(_queueName);
                    _queueName = value;
                    Handle();
                }
            }
        }

        public FileReceiverService(RabbitMQService rabbit, ILogger<FileReceiverService> logger)
        {
            _rabbit = rabbit;
            _logger = logger;
            Handle();
        }

        private void Handle()
        {
            _rabbit.Register(QueueName, (model, ea) =>
            {
                try
                {
                    var fileId = ea.BasicProperties.CorrelationId;
                    var chunkNumber = Convert.ToInt32(ea.BasicProperties.Headers ["chunkNumber"]);
                    var totalBytes = Convert.ToInt64(ea.BasicProperties.Headers ["totalBytes"]);
                    var body = ea.Body.ToArray();

                    if (!_fileStream.ContainsKey(fileId))
                    {
                        _fileStream [fileId] = (new MemoryStream(), totalBytes, 0);
                    }

                    var (stream, _, chunksReceived) = _fileStream [fileId];
                    stream.Write(body, 0, body.Length);
                    _fileStream [fileId] = (stream, totalBytes, chunksReceived + 1);

                    _rabbit.Channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);

                    if (stream.Length == totalBytes)
                    {
                        _logger.LogTrace("File with ID {fileId} has been completely received.", fileId);
                        FileCompleted?.Invoke(stream, Guid.Parse(fileId));

                        stream.Dispose();
                        _fileStream.Remove(fileId);
                    }
                }
                catch (Exception ex)
                {
                    _rabbit.Channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
                    _logger.LogError(ex, "Failed to process chunk: {ex.Message}", ex.Message);
                }
            });
        }
    }
}
