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

        [HttpGet("DropDown")]
        public async Task<Response<DropdownPlaceDTO>> GetDropDownMenu()
        {
            var result = await _internalService.GetAsync<DropdownPlaceDTO>(ServiceType.Spot, "Place/InitPlaceData");
            return result;
        }
    }
}
