using CommonService.Constants;
using CommonService.Services;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaceController(IInternalService internalService) : ControllerBase
    {
        private readonly IInternalService _internalService = internalService;

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
    }
}
