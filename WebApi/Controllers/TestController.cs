using CommonService.Extentions;
using CommonService.RPC;
using CommonService.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Text;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController(FileProviderService fileProvider, RpcClient rpcClient, ILogger<TestController> logger) : Controller
    {
        private readonly FileProviderService _fileProvider = fileProvider;
        private readonly RpcClient _rpcClient = rpcClient;

        [HttpPost]
        public async Task<IActionResult> PostFile(IFormFile file)
        {
            using var fileStream = file.OpenReadStream();
            int bytesRead;
            int chunkNumber = 0;
            byte [] buffer = new byte [100 * 1024]; // 100KB chunks
            var sb = new StringBuilder();
            var fileId = Guid.NewGuid().ToString();
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                var body = new byte [bytesRead];
                Array.Copy(buffer, body, bytesRead);

                var headers = new Dictionary<string, object>
                    {
                        { "fileId", fileId},
                        { "chunkNumber", chunkNumber },
                        { "totalBytes", fileStream.Length }
                    };
                var result = await _rpcClient.CallAsync(body, "image-service", "store", headers);
                if (result.Contains("Stored"))
                {
                    sb.AppendLine(result);
                }
                chunkNumber++;
            }
            return Ok(JsonConvert.DeserializeObject(sb.ToString()));
        }

        [HttpGet]
        public async Task<IActionResult> GetLink([FromQuery] Guid guid)
        {
            var json = JsonConvert.SerializeObject(new { Id = guid });
            var res = await _rpcClient.CallAsync(json, "image-service", "get-link");
            return Ok(JsonConvert.DeserializeObject(res));
        }
    }
}
