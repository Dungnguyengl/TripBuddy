using CommonService.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using SpotService.Model;

namespace SpotService.Controllers.Atraction
{
    [ApiController]
    [Route("api/[controller]")]
    public class AtractionController(SpotDbContext context) : ControllerBase
    {
        private readonly SpotDbContext _context = context;

        [HttpGet("Keys")]
        public ActionResult<AttractionKeyValueDTO> GetAttractionKeyValue()
        {
            var keyValuePairs = from attr in _context.AtrHeads.AsNoTracking()
                                select new AttractionKeyValueDTO
                                {
                                    AtrKey = attr.AtrKey,   
                                    Country = attr.Country
                                };

            return Ok(keyValuePairs);
        }

        [HttpGet("Details")]
        public ActionResult<DAttractionDTO> GetAttractionDetail([FromQuery]Guid atrId)
        {
            if (string.IsNullOrEmpty(atrId.ToString()))
            {
                throw new Exception("Id is empty");
            }

            // Load the main attraction
            var attraction = (from head in _context.AtrHeads.AsNoTracking()
                              where head.AtrKey == atrId
                              join continent in _context.Constants.AsNoTracking() on head.Contitnent equals continent.ConstantCode
                              join sub in _context.Constants.AsNoTracking() on head.SubContitnent equals sub.ConstantCode
                              select new DAttractionDTO
                              {
                                  AtrKey = head.AtrKey,
                                  Contitnent = continent.ConstantName,
                                  SubContitnent = sub.ConstantName,
                                  Description = head.Description,
                                  Country = head.Country
                              }).FirstOrDefault() ?? throw new NotFoundException($"Atraction - {atrId}");

            //attraction.AtrContents = _context.AtrContents.AsNoTracking()
            //    .Where(content => content.AtrKey == atrId)
            //    .Select(content => new AtrContent
            //    {
            //        ContentKey = content.ContentKey,
            //        Content = content.Content,
            //        Title = content.Title,
            //        ContentType = content.ContentType
            //    }).ToList();

            //attraction.DesHeads = _context.DesHeads.AsNoTracking()
            //    .Where(des => des.AtrKey == atrId)
            //    .Select(des => new DesHead
            //    {
            //        DesKey = des.DesKey,
            //        Description = des.Description,
            //        DesName = des.DesName
            //    }).Take(6).ToList();

            //attraction.PlcHeads = _context.PlcHeads.AsNoTracking()
            //    .Where(plc => plc.AtrKey == atrId)
            //    .Select(plc => new PlcHead
            //    {
            //        PlcKey = plc.PlcKey,
            //        Description = plc.Description,
            //        PlcName = plc.PlcName
            //    }).Take(6).ToList();

            return attraction;
        }

    }
}
