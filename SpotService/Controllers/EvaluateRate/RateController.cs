using CommonService.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotService.Model;

namespace SpotService.Controllers.EvaluateRate
{
    [ApiController]
    [Route("api/[controller]")]
    public class RateController(EvaluateDbContext eContext, SpotDbContext sContext) : ControllerBase
    {
        private readonly EvaluateDbContext _eContext = eContext;
        private readonly SpotDbContext _sContext = sContext;

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int top = 5)
        {
            var evlPlc = _eContext.EvlPlcs.AsNoTracking();

            var query = evlPlc
                .OrderByDescending(x => x.AverageRate)
                .Select(x => new RateDTO
                {
                    PlcKey = x.PlcKey,
                    PicKey = x.PicKey,
                    AtrName = x.AtrName,
                    DesName = x.DesName,
                    PlcName = x.PlcName,
                    PlcDescription = x.PlcDescription,
                    AverageRate = x.AverageRate.HasValue ? ((float) x.AverageRate.Value) : 0,
                });

            return Ok(await query.ToListAsync());
        }

        //[HttpGet("Details")]
        //public async Task<ActionResult<RateDTO>> Details([FromQuery] Guid rateKey)
        //{
        //var query = from head in _context.EvlHeads.AsNoTracking()
        //            where head.EvlKey == rateKey
        //            join rate in _context.EvlRates.AsNoTracking() on head.EvlKey equals rate.EvlKey
        //            join ePlc in _context.EvlPlcs.AsNoTracking() on head.EvlKey equals ePlc.EvlKey
        //            join plc in _context.PlcHeads.AsNoTracking() on ePlc.PlcKey equals plc.PlcKey
        //            join des in _context.DesHeads.AsNoTracking() on ePlc.DesKey equals des.DesKey
        //            join att in _context.AtrContents.AsNoTracking() on ePlc.AtrKey equals att.AtrKey
        //            select new RateDTO
        //            {
        //                EvlKey = head.EvlKey,
        //                EvlTitle = head.EvlTitle,
        //                EvlDescription = head.EvlContent,
        //                AtrTitle = att.Title ?? string.Empty,
        //                DesTitle = des.DesName ?? string.Empty,
        //                Rate = rate.Rate,
        //                PicKey = head.PicKey,
        //                PlcTitle = plc.PlcName ?? string.Empty
        //            };

        //var result = await query.FirstOrDefaultAsync();
        //if (result == null) return NotFound();

        //return Ok(result);
        //}

        [HttpPost]
        public async Task<ActionResult<RateDTO>> Create([FromBody] CRateDTO rateDTO)
        {
            if (rateDTO == null)
            {
                return BadRequest("Invalid rate data.");
            }

            var place = await _sContext.PlcHeads.AsNoTracking()
                .FirstOrDefaultAsync(p => p.PlcKey == rateDTO.PlcKey) ?? throw new NotFoundException($"Place {rateDTO.PlcKey}");

            var attraction = await _sContext.AtrHeads.AsNoTracking()
                .FirstOrDefaultAsync(a => a.AtrKey == place.AtrKey) ?? throw new NotFoundException($"Atraction of Place {rateDTO.PlcKey}");
            var destination = await _sContext.DesHeads.AsNoTracking()
                .FirstOrDefaultAsync(d => d.DesKey == place.DesKey) ?? throw new NotFoundException($"Destination of Place {rateDTO.PlcKey}");

            var rateHead = new EvlHead
            {
                EvlContent = rateDTO.EvlDescription,
                EvlTitle = rateDTO.EvlTitle,
                EvlKey = Guid.NewGuid(),
                PicKey = rateDTO.PicKey,
                PlcKey = rateDTO.PicKey,
                Rate = ((short) rateDTO.Rate)
            };

            _eContext.EvlHeads.Add(rateHead);
            await _eContext.SaveChangesAsync();

            return CreatedAtAction(nameof(EvlHead), new { rateKey = rateHead.EvlKey });
        }
    }
}
