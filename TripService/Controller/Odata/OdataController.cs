using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TripperService.Model;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using TripperService.Controller.TripController;

namespace TripperService.Controller.Odata
{
    public class OdataController(TripperDbContext context) : ODataController
    {
        private readonly TripperDbContext _context = context;

        [HttpGet("api/Trip")]
        public IActionResult GetAllTrip()
        {
            var trips = from head in _context.TripHeads.AsNoTracking()
                                  where head.IsDeleted == false    
                        join place in _context.TripPlcs.AsNoTracking()
                             on head.TripKey equals place.TripKey
                        select new TripDTO
                        {
                            EndDate = head.EndDate,
                            PlcContent = place.PlcContent,
                            PlcType = place.PlcType,
                            ReferentKey = place.ReferentKey,
                            StartDate = head.StartDate,
                            TripKey = head.TripKey,
                        };

            return Ok(trips);
        }

        [HttpGet("api/Trip/History")]
        public IActionResult GetHistory()
        {
            var trips = from head in _context.TripHeads.AsNoTracking()
                        where head.IsDeleted == false
                        where head.IsVisited == true
                        join place in _context.TripPlcs.AsNoTracking()
                             on head.TripKey equals place.TripKey
                        select new TripDTO
                        {
                            EndDate = head.EndDate,
                            PlcContent = place.PlcContent,
                            PlcType = place.PlcType,
                            ReferentKey = place.ReferentKey,
                            StartDate = head.StartDate,
                            TripKey = head.TripKey,
                        };

            return Ok(trips);
        }
    }
}
