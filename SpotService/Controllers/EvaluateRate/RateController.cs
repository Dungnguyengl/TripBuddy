using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotService.Controllers.Destination;
using SpotService.Model;

namespace SpotService.Controllers.EvaluateRate
{
    [Route("api/[controller]")]
    public class RateController(SpotDbContext context) : ControllerBase
    {
        private readonly SpotDbContext _context = context;
        [HttpGet]
        public Task<List<RateDTO>> Get()
        {
            var query = from head in _context.EvlHeads.AsNoTracking()
                        join rate in _context.EvlRates.AsNoTracking()
                        on head.EvlKey equals rate.EvlKey
                        join ePlc in _context.EvlPlcs.AsNoTracking()
                        on head.EvlKey equals ePlc.EvlKey
                        join plc in _context.PlcHeads.AsNoTracking()
                        on ePlc.PlcKey equals plc.PlcKey
                        join des in _context.DesHeads.AsNoTracking()
                        on ePlc.DesKey equals des.DesKey
                        join att in _context.AtrContents.AsNoTracking()
                        on ePlc.AtrKey equals att.AtrKey
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

            return Task.FromResult(query.ToList());
        }


        [HttpGet("Details")]
        public ActionResult<RateDTO> Details([FromQuery]Guid rateKey)
        {
            var query = from head in _context.EvlHeads.AsNoTracking()
                        where head.EvlKey == rateKey
                        join rate in _context.EvlRates.AsNoTracking()
                        on head.EvlKey equals rate.EvlKey
                        join ePlc in _context.EvlPlcs.AsNoTracking()
                        on head.EvlKey equals ePlc.EvlKey
                        join plc in _context.PlcHeads.AsNoTracking()
                        on ePlc.PlcKey equals plc.PlcKey
                        join des in _context.DesHeads.AsNoTracking()
                        on ePlc.DesKey equals des.DesKey
                        join att in _context.AtrContents.AsNoTracking()
                        on ePlc.AtrKey equals att.AtrKey
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

            if (query.Any())
            {
                return Ok(query);
            }
            else return NotFound();
        }

        [HttpPost]
        public async Task<RateDTO> Create([FromBody] CRateDTO rateDTO)
        {
            if (rateDTO == null)
            {
                throw new Exception(nameof(Create) + " Model is null or invalid");
            }

            var place = _context.PlcHeads.Where(item => item.PlcKey == rateDTO.PlcKey)
                                         .AsNoTracking()                         
                                         .FirstOrDefault()
                                         ?? throw new NullReferenceException();

            var attraction = _context.AtrContents.Where(item => item.AtrKey == place.AtrKey)
                                                 .AsNoTracking()
                                                 .FirstOrDefault()
                                                 ?? throw new NullReferenceException();
            
            var destination = _context.DesHeads.Where(item => item.DesKey == place.DesKey)
                                               .AsNoTracking()
                                               .FirstOrDefault()
                                               ?? throw new NullReferenceException();

            var rateHead = new EvlHead
            {
                EvlContent = rateDTO.EvlDescription,
                EvlTitle = rateDTO.EvlTitle,
                EvlKey = Guid.NewGuid(),
                PicKey = rateDTO.PicKey
            };

            var rate = new EvlRate { 
                EvlKey = rateHead.EvlKey,
                Rate = rateDTO.Rate,
            };

            var ratePlace = new EvlPlc
            {
                EvlKey = rate.EvlKey,
                DesKey = destination.DesKey,
                AtrKey = attraction.AtrKey,
                PlcKey = place.PlcKey,
                PlcContent = place.PlcName ?? string.Empty,
            };

            try
            {
                _context.EvlHeads.Add(rateHead);
                _context.EvlRates.Add(rate);
                _context.EvlPlcs.Add(ratePlace);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in create Review");
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

            return returnDTO;
        }
    }
}
