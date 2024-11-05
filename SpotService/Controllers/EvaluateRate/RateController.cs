using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotService.Model;

namespace SpotService.Controllers.EvaluateRate
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateController : ControllerBase
    {
        private readonly SpotDbContext _context;

        public RateController(SpotDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<RateDTO>>> Get()
        {
            var query = from head in _context.EvlHeads.AsNoTracking()
                        join rate in _context.EvlRates.AsNoTracking() on head.EvlKey equals rate.EvlKey
                        join ePlc in _context.EvlPlcs.AsNoTracking() on head.EvlKey equals ePlc.EvlKey
                        join plc in _context.PlcHeads.AsNoTracking() on ePlc.PlcKey equals plc.PlcKey
                        join des in _context.DesHeads.AsNoTracking() on ePlc.DesKey equals des.DesKey
                        join att in _context.AtrContents.AsNoTracking() on ePlc.AtrKey equals att.AtrKey
                        select new RateDTO
                        {
                            EvlKey = head.EvlKey,
                            EvlTitle = head.EvlTitle,
                            EvlDescription = head.EvlContent,
                            AtrTitle = att.Title ?? string.Empty,
                            DesTitle = des.DesName ?? string.Empty,
                            Rate = rate.Rate,
                            PicKey = head.PicKey,
                            PlcTitle = plc.PlcName ?? string.Empty
                        };

            return await query.ToListAsync();
        }

        [HttpGet("Details")]
        public async Task<ActionResult<RateDTO>> Details([FromQuery] Guid rateKey)
        {
            var query = from head in _context.EvlHeads.AsNoTracking()
                        where head.EvlKey == rateKey
                        join rate in _context.EvlRates.AsNoTracking() on head.EvlKey equals rate.EvlKey
                        join ePlc in _context.EvlPlcs.AsNoTracking() on head.EvlKey equals ePlc.EvlKey
                        join plc in _context.PlcHeads.AsNoTracking() on ePlc.PlcKey equals plc.PlcKey
                        join des in _context.DesHeads.AsNoTracking() on ePlc.DesKey equals des.DesKey
                        join att in _context.AtrContents.AsNoTracking() on ePlc.AtrKey equals att.AtrKey
                        select new RateDTO
                        {
                            EvlKey = head.EvlKey,
                            EvlTitle = head.EvlTitle,
                            EvlDescription = head.EvlContent,
                            AtrTitle = att.Title ?? string.Empty,
                            DesTitle = des.DesName ?? string.Empty,
                            Rate = rate.Rate,
                            PicKey = head.PicKey,
                            PlcTitle = plc.PlcName ?? string.Empty
                        };

            var result = await query.FirstOrDefaultAsync();
            if (result == null) return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<RateDTO>> Create([FromBody] CRateDTO rateDTO)
        {
            var userId = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var picKey))
            {
                return BadRequest("Invalid or missing user ID.");
            }

            if (rateDTO == null)
            {
                return BadRequest("Invalid rate data.");
            }

            var place = await _context.PlcHeads.AsNoTracking().FirstOrDefaultAsync(p => p.PlcKey == rateDTO.PlcKey);
            if (place == null) return NotFound("Place not found.");

            var attraction = await _context.AtrContents.AsNoTracking().FirstOrDefaultAsync(a => a.AtrKey == place.AtrKey);
            var destination = await _context.DesHeads.AsNoTracking().FirstOrDefaultAsync(d => d.DesKey == place.DesKey);

            if (attraction == null || destination == null) return NotFound("Related attraction or destination not found.");

            var rateHead = new EvlHead
            {
                EvlContent = rateDTO.EvlDescription,
                EvlTitle = rateDTO.EvlTitle,
                EvlKey = Guid.NewGuid(),
                PicKey = picKey
            };

            var rate = new EvlRate
            {
                EvlKey = rateHead.EvlKey,
                Rate = rateDTO.Rate
            };

            var ratePlace = new EvlPlc
            {
                EvlKey = rate.EvlKey,
                DesKey = destination.DesKey,
                AtrKey = attraction.AtrKey,
                PlcKey = place.PlcKey,
                PlcContent = place.PlcName ?? string.Empty
            };

            try
            {
                _context.EvlHeads.Add(rateHead);
                _context.EvlRates.Add(rate);
                _context.EvlPlcs.Add(ratePlace);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while creating the review.");
            }

            var returnDTO = new RateDTO
            {
                EvlKey = rateHead.EvlKey,
                EvlTitle = rateHead.EvlTitle,
                EvlDescription = rateHead.EvlContent,
                AtrTitle = attraction.Title ?? string.Empty,
                DesTitle = destination.DesName ?? string.Empty,
                Rate = rate.Rate,
                PicKey = rateHead.PicKey,
                PlcTitle = place.PlcName ?? string.Empty
            };

            return CreatedAtAction(nameof(Details), new { rateKey = returnDTO.EvlKey }, returnDTO);
        }
    }
}
