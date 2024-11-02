using CommonService.Constants;
using CommonService.Extentions;
using CommonService.Services;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AtractionController(IInternalService internalService) : ControllerBase
    {
        private readonly IInternalService _internalService = internalService;

        [HttpGet("search")]
        public async Task<ODataRespose<SearchAtractionDto>> GetAllAtraction([FromQuery] SearchAtractionQuery query)
        {
            var param = new ODataParam(count: true);
            if (!query.Continent.IsNullOrEmpty())
            {
                param.AddFilter((filter) =>
                {
                    filter.WithOr(new("continent", ODataConstants.CONTAIN, query.Continent));
                    filter.WithOr(new("subContinent", ODataConstants.CONTAIN, query.Continent));
                });
            }
            if (!query.Word.IsNullOrEmpty())
            {
                param.AddFilter(filter =>
                {
                    filter.WithOr(new("country", ODataConstants.EQUAL, query.Word));
                });
            }
            return await _internalService.GetByODataAsync<SearchAtractionDto>(ServiceType.Spot, param, path: "Atraction");
        }
    }
}
