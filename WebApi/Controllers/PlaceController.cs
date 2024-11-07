using CommonService.Constants;
using CommonService.RPC;
using CommonService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using WebApi.Dtos;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaceController(IInternalService internalService, RpcClient rpcClient) : ControllerBase
    {
        private readonly IInternalService _internalService = internalService;
        private readonly RpcClient _rpcClient = rpcClient;

        [HttpGet("detail")]
        public async Task<Response<DetailPlaceDto>> GetDetail([FromQuery] DetailPlaceQuery query)
        {
            var result = await _internalService.GetAsync<DetailPlaceDto>(ServiceType.Spot, query, "Place/Details");
            return result;
        }

        [HttpGet("InitPlaceData")]
        public async Task<Response<DropdownPlaceDTO>> GetDetail()
        {
            var result = await _internalService.GetAsync<DropdownPlaceDTO>(ServiceType.Spot, "Place/InitPlaceData");
            return result;
        }

        [HttpGet("GetAll")]
        public async Task<ODataRespose<DetailPlaceDto>> GetAllPlace([FromQuery] SearchPlaceDto query)
        {
            var param = new ODataParam(count: true);
            param.Top = query.Items;
            param.Skip = query.Skips;
            return await _internalService.GetByODataAsync<DetailPlaceDto>(ServiceType.Spot, param, path: "Destination");
        }

        [HttpPost]
        public async Task CreatePlace([FromForm] CreatePlaceCommand command, IFormFile file)
        {
            var picKey = Guid.NewGuid().ToString();
            var places = await _internalService.PostAsync<object>(ServiceType.Spot, new
            {
                command.DesKey,
                command.AtrKey,
                command.PlaceName,
                command.Description,
                PicKey = picKey
            }, "Place");

            using var fileStream = file.OpenReadStream();
            int bytesRead;
            int chunkNumber = 0;
            byte [] buffer = new byte [100 * 1024]; // 100KB chunks
            var sb = new StringBuilder();
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                var body = new byte [bytesRead];
                Array.Copy(buffer, body, bytesRead);

                var headers = new Dictionary<string, object>
                    {
                        { "fileId", picKey.ToString()},
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
            var image = sb.ToString();
        }
    }
}
