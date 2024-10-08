using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using SpotService.Model;

namespace SpotService.Controllers.Atraction
{
    public class AtractionController(SpotDbContext context) : ODataController
    {
        private readonly SpotDbContext _context = context;

        [EnableQuery]
        public IActionResult Get()
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
    }
}
