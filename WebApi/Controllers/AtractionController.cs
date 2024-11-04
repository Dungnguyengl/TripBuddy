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
            param.Top = query.Items;
            param.Skip = query.Skips;
            return await _internalService.GetByODataAsync<SearchAtractionDto>(ServiceType.Spot, param, path: "Atraction");
        }

        [HttpGet("detail")]
        public async Task<Response<AtractionDetailDto>> GetDetailAtraction([FromQuery] AtractionDetailQuery query)
        {
            var atraction = await _internalService.GetAsync<AtractionDetailDto>(ServiceType.Spot, query, "Atraction/Details");
            if (atraction.Code == System.Net.HttpStatusCode.NotFound)
            {
                return atraction;
            }
            var oDataParams = new ODataParam(top: 6);
            oDataParams.AddFilter(filter => filter.WithAnd(new("atrKey", ODataConstants.EQUAL, query.AtrId)));
            var destinations = await _internalService.GetByODataAsync<ChildDestinationDto>(ServiceType.Spot, oDataParams, "Destination");
            var places = await _internalService.GetByODataAsync<ChildPlaceDto>(ServiceType.Spot, oDataParams, "Place");
            var stories = await _internalService.GetByODataAsync<ChildStoryDto>(ServiceType.Spot, oDataParams, "Story");
            if (destinations.Code == System.Net.HttpStatusCode.OK)
            {
                atraction.Content.Destinations = destinations.Value;
            }
            if (places.Code == System.Net.HttpStatusCode.OK)
            {
                atraction.Content.Places = places.Value;
            }
            if (stories.Code == System.Net.HttpStatusCode.OK)
            {
                atraction.Content.Stories = stories.Value;
            }

            return atraction;
        }
    }
}
