using CommonService.RPC;
using CommonService.Services;
using ImageService.Dtos;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace ImageService.Services
{
    public class ImageRpcService(RabbitMQService rabbit, ILogger<ImageRpcService> logger, S3Service s3) : RpcServiceBase("image-service", rabbit, logger)
    {
        private readonly S3Service _s3Service = s3;
        private readonly Dictionary<string, (MemoryStream stream, long totalBytes, int chunksReceived)> _fileStream = [];

        protected override string Handle(string message, string type, IDictionary<string, object> headers)
        {
            dynamic result = string.Empty;
            switch (type.ToLower())
            {
                case "get-link":
                {
                    var obj = JsonConvert.DeserializeObject<GetImageLinkQuery>(message);
                    result = new GetImageLinkDto();
                    if (obj != null)
                    {
                        result.LinkImage = _s3Service.GeneratePreSignedURL(obj.Id);
                    }
                    break;
                }
                case "delete":
                {
                    var obj = JsonConvert.DeserializeObject<DeleteImageCommand>(message);
                    result = new DeleteImageDto();
                    if (obj != null)
                    {
                        try
                        {
                            _s3Service.DeleteImage(obj.Id).Wait();
                            result.Message = $"Deleted image {obj.Id} success!";
                        }
                        catch
                        {
                            result.Message = $"Fail to delete image {obj.Id}";
                        }
                    }
                    break;
                }
                case "restore":
                {
                    var obj = JsonConvert.DeserializeObject<DeleteImageCommand>(message);
                    result = new DeleteImageDto();
                    if (obj != null)
                    {
                        try
                        {
                            _s3Service.RestoreImage(obj.Id).Wait();
                            result.Message = $"Restore image {obj.Id} success!";
                        }
                        catch
                        {
                            result.Message = $"Fail to restore image {obj.Id}";
                        }
                    }
                    break;
                }
            }

            return (result is string) ? result : JsonConvert.SerializeObject(result);
        }

        protected override string Handle(byte [] body, string type, IDictionary<string, object> headers)
        {
            dynamic result = string.Empty;
            switch (type.ToLower())
            {
                case "store":
                {
                    Task.Run(async () =>
                    {
                        result = await HandleStoreImage(body, headers);
                    }).Wait();
                    break;
                }
            }
            return (result is string) ? result : JsonConvert.SerializeObject(result);
        }

        private async Task<dynamic> HandleStoreImage(byte [] body, IDictionary<string, object> headers)
        {
            var fileId = Encoding.UTF8.GetString((byte []) headers ["fileId"]) ?? string.Empty;
            var chunkNumber = Convert.ToInt32(headers ["chunkNumber"]);
            var totalBytes = Convert.ToInt64(headers ["totalBytes"]);

            if (!_fileStream.ContainsKey(fileId))
            {
                _fileStream [fileId] = (new MemoryStream(), totalBytes, 0);
            }

            var (stream, _, chunksReceived) = _fileStream [fileId];
            stream.Write(body, 0, body.Length);
            _fileStream [fileId] = (stream, totalBytes, chunksReceived + 1);

            if (stream.Length == totalBytes)
            {
                await s3.StoreImageAsync(stream, Guid.Parse(fileId));

                stream.Dispose();
                _fileStream.Remove(fileId);
                return new { Message = $"Stored {fileId}" };
            }

            return new { Message = $"Storeing {fileId}" };
        }
    }
}
