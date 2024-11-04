using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using SpotService.Controllers.Atraction;
using SpotService.Controllers.Destination;
using SpotService.Controllers.Place;
using SpotService.Controllers.Story;
using SpotService.Model;

namespace SpotService.Controllers.Odata
{
    public class OdataController(SpotDbContext context) : ODataController
    {
        private readonly SpotDbContext _context = context;

        [EnableQuery]
        [HttpGet("api/atraction")]
        public IActionResult GetAtraction()
        {
            var atrHeads = _context.AtrHeads.AsNoTracking();
            var constants = _context.Constants.AsNoTracking();

            var query = from head in atrHeads
                        join continent in constants on head.Contitnent equals continent.ConstantCode
                        join sub in constants on head.SubContitnent equals sub.ConstantCode
                        select new AtractionDTO
                        {
                            Id = head.AtrKey,
                            Continent = continent.ConstantName,
                            SubContinent = sub.ConstantName,
                            Country = head.Country,
                            PicLink = ""
                        };
            return Ok(query);
        }

        [EnableQuery]
        [HttpGet("api/destination")]
        public IActionResult GetDestination()
        {
            var query = from head in _context.DesHeads.AsNoTracking()
                        select new DestinationDTO
                        {
                            DesKey = head.DesKey,
                            DesDescription = head.Description ?? string.Empty,
                            DesName = head.DesName ?? string.Empty,
                            DesPic = head.PicKey,
                            AtrKey = head.AtrKey,
                        };
            return Ok(query);
        }

        [EnableQuery]
        [HttpGet("api/place")]
        public IActionResult GetPlace()
        {
            var query = from head in _context.PlcHeads.AsNoTracking()
                        select new PlaceDTO
                        {
                            PlaceKey = head.PlcKey,
                            AtrKey = head.AtrKey,
                            Description = head.Description,
                            DesKey = head.DesKey,
                            Latitude = head.Latitude,
                            Longitude = head.Longitude,
                            PicKey = head.PicKey,
                            PlaceName = head.PlcName,
                        };
            return Ok(query);
        }

        [EnableQuery]
        [HttpGet("api/story")]
        public IActionResult GetStory()
        {
            var atractions = _context.AtrHeads.AsNoTracking();
            var destinations = _context.DesHeads.AsNoTracking();
            var stories = _context.AtrContents.AsNoTracking();

            var query = from story in stories
                           join atraction in atractions on story.AtrKey equals atraction.AtrKey
                           join destination in destinations on story.DesKey equals destination.DesKey
                           select new StoryDTO
                           {
                               StoryKey = story.ContentKey,
                               AtrKey = atraction.AtrKey,
                               Country = atraction.Country,
                               Destination = destination.DesName,
                               Title = story.Title,
                               Content = story.Content
                           };

            return Ok(query);
        }
    }
}
