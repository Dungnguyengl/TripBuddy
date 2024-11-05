using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TripperService.Model;

namespace TripperService.Controller.TripController
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly TripperDbContext _context;

        public TripController(TripperDbContext context)
        {
            _context = context;
        }

        // Create a Schedule
        [HttpPost]
        public async Task<ActionResult> CreateSchedule([FromBody] CTripDTO tripDTO)
        {
            if (tripDTO == null || !ModelState.IsValid)
                return BadRequest("Invalid trip data.");

            if (!Guid.TryParse(User.FindFirstValue("UserId"), out var picKey))
                return BadRequest("Invalid or missing user ID.");

            var tripHead = new TripHead
            {
                TripKey = Guid.NewGuid(),
                StartDate = tripDTO.StartDate,
                EndDate = tripDTO.EndDate,
                IsDeleted = false,
                IsVisited = false,
                PicKey = picKey
            };

            var tripPlc = new TripPlc
            {
                TripKey = tripHead.TripKey,
                PlcType = tripDTO.PlcType,
                PlcContent = tripDTO.PlcContent,
                ReferentKey = tripDTO.ReferentKey
            };

            _context.TripHeads.Add(tripHead);
            _context.TripPlcs.Add(tripPlc);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTripById), new { tripKey = tripHead.TripKey }, tripHead);
        }

        // Get Trip Details by ID
        [HttpGet("Details")]
        public async Task<ActionResult<TripDTO>> GetTripById([FromQuery] Guid tripKey)
        {
            var trip = await (from head in _context.TripHeads.AsNoTracking()
                              where head.TripKey == tripKey && !head.IsDeleted
                              join place in _context.TripPlcs.AsNoTracking() on head.TripKey equals place.TripKey
                              select new TripDTO
                              {
                                  TripKey = head.TripKey,
                                  StartDate = head.StartDate,
                                  EndDate = head.EndDate,
                                  PlcType = place.PlcType,
                                  PlcContent = place.PlcContent,
                                  ReferentKey = place.ReferentKey
                              }).FirstOrDefaultAsync();

            return trip != null ? Ok(trip) : NotFound();
        }

        // Update Schedule
        [HttpPut]
        public async Task<ActionResult> UpdateSchedule([FromBody] UTripDTO tripDTO)
        {
            if (tripDTO == null || !ModelState.IsValid)
                return BadRequest("Invalid trip data.");

            if (!Guid.TryParse(User.FindFirstValue("UserId"), out var picKey))
                return BadRequest("Invalid or missing user ID.");

            var tripHead = new TripHead
            {
                TripKey = tripDTO.TripKey,
                StartDate = tripDTO.StartDate,
                EndDate = tripDTO.EndDate,
                PicKey = picKey,
                IsVisited = false,
                IsDeleted = false
            };

            var tripPlc = new TripPlc
            {
                TripKey = tripDTO.TripKey,
                PlcType = tripDTO.PlcType,
                PlcContent = tripDTO.PlcContent,
                ReferentKey = tripDTO.ReferentKey
            };

            _context.TripHeads.Update(tripHead);
            _context.TripPlcs.Update(tripPlc);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Check in Schedule
        [HttpPut("CheckIn")]
        public async Task<ActionResult> CheckIn([FromRoute] Guid tripKey)
        {
            var trip = await _context.TripHeads.FindAsync(tripKey);

            if (trip == null)
                return NotFound("Trip not found.");

            trip.IsVisited = true;
            _context.TripHeads.Update(trip);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Delete Schedule
        [HttpDelete]
        public async Task<ActionResult> DeleteSchedule([FromQuery] Guid tripKey)
        {
            var tripHead = await _context.TripHeads.FindAsync(tripKey);

            if (tripHead == null)
                return NotFound("Trip not found.");

            tripHead.IsDeleted = true;
            _context.TripHeads.Update(tripHead);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
