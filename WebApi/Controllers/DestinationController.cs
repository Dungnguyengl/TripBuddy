using CommonService.Constants;
using CommonService.RPC;
using CommonService.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using WebApi.Dtos;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DestinationController(IInternalService internalService, RpcClient rpcClient) : ControllerBase
    {
        private readonly IInternalService _internalService = internalService;
        private readonly RpcClient _rpcClient = rpcClient;

        [HttpGet("Details")]
        public async Task<Response<DestinationDetailDTO>> GetDetail([FromQuery] DetailPlaceQuery query)
        {
            var result = await _internalService.GetAsync<DestinationDetailDTO>(ServiceType.Spot, query, "Destination/Details");
            var param = new ODataParam(count: true);
            param.AddFilter(x => x.WithAnd(new("DesKey", ODataConstants.EQUAL, query.Key)));
            var places = await _internalService.GetByODataAsync<DetailPlaceDto>(ServiceType.Spot, param, "place");
            if (places.Code == System.Net.HttpStatusCode.OK)
            {
                result.Content.Places = [.. (await Task.WhenAll([.. places.Value.Select(async x => {
                    var picLink = "";
                    if (x.PicKey.HasValue){
                        var image = await _rpcClient.CallAsync(JsonConvert.SerializeObject(new { Id = x.PicKey }), "image-service", "get-link");
                        picLink = JsonConvert.DeserializeObject<dynamic>(image)?.linkImage;
                    }
                    x.PicLink = picLink;
                    return x;
                })]))];
            }
            return result;
        }

        [HttpGet("GetAll")]
        public async Task<ODataRespose<DestinationDTO>> GetAllDestination([FromQuery] SearcDestinationDTO query)
        {
            var param = new ODataParam(count: true);
            param.Top = query.Items;
            param.Skip = query.Skips;
            return await _internalService.GetByODataAsync<DestinationDTO>(ServiceType.Spot, param, path: "Destination");
        }
    }
}
