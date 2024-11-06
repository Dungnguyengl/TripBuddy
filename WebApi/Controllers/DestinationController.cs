using CommonService.Constants;
using CommonService.Services;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DestinationController(IInternalService internalService) : ControllerBase
    {
        private readonly IInternalService _internalService = internalService;

        [HttpGet("Details")]
        public async Task<Response<DestinationDTO>> GetDetail([FromQuery] DetailPlaceQuery query)
        {
            var result = await _internalService.GetAsync<DestinationDTO>(ServiceType.Spot, query, "Destination/Details");
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
