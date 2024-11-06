using CommonService.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using SpotService.Controllers.Destination;
using SpotService.Model;

namespace SpotService.Controllers.Place
{
    [Route("api/[controller]")]
    public class PlaceController(SpotDbContext context) : ControllerBase
    {
        private readonly SpotDbContext _context = context;

        [HttpGet("Details")]
        public ActionResult<PlaceDTO> Details([FromQuery] Guid key)
        {
            var query = from head in _context.PlcHeads.AsNoTracking()
                        where head.PlcKey == key
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

            if (query.Any())
            {
                return Ok(query.FirstOrDefault());
            }
            else throw new NotFoundException($"Place: {key}");
        }

        [HttpGet("Search")]
        public ActionResult<List<PlaceDTO>> Search([FromQuery] string search)
        {
            search = search.ToUpper();

            var attractionsQuery = _context.AtrHeads.AsNoTracking()
                .Where(item => item.Contitnent.ToUpper().Contains(search)
                            || item.SubContitnent.ToUpper().Contains(search)
                            || item.Country.ToUpper().Contains(search))
                .Select(item => item.AtrKey);

            var desHeadQuery = _context.DesHeads.AsNoTracking()
                .Where(item => item.DesName.ToUpper().Contains(search))
                .Select(item => item.DesKey);

            var plcHeadQuery = _context.PlcHeads.AsNoTracking()
                .Where(item => item.PlcName.ToUpper().Contains(search)
                            || attractionsQuery.Contains(item.AtrKey)
                            || desHeadQuery.Contains(item.DesKey));

            // Final projection to DTO
            var query = plcHeadQuery.Select(head => new PlaceDTO
            {
                AtrKey = head.AtrKey,
                DesKey = head.DesKey,
                Description = head.Description,
                Latitude = head.Latitude,
                Longitude = head.Longitude,
                PicKey = head.PicKey,
                PlaceKey = head.PlcKey,
                PlaceName = head.PlcName
            });

            // Return the result
            return Ok(query.ToList());
        }

        [HttpGet("Keys")]
        public ActionResult<PlaceKeyValueDTO> GetPlaceKeyValue()
        {
            var keyValuePairs = from des in _context.PlcHeads.AsNoTracking()
                                select new PlaceKeyValueDTO
                                {
                                    PlcKey = des.PlcKey,
                                    PlcName = des.PlcName,
                                };

            return Ok(keyValuePairs);
        }

        [HttpPost]
        public async Task<PlaceDTO> Create([FromBody] CPlaceDTO placeDTO)
        {
            if (placeDTO == null)
            {
                throw new Exception(nameof(Create) + " Model is null or invalid");
            }

            var attraction = _context.AtrHeads.Where(item => item.AtrKey == placeDTO.AtrKey)
                                              .AsNoTracking()
                                              .FirstOrDefault()
                                              ?? throw new ArgumentNullException();

            var destination = _context.DesHeads.Where(item => item.DesKey == placeDTO.DesKey)
                                              .AsNoTracking()
                                              .FirstOrDefault()
                                              ?? throw new ArgumentNullException();

            var place = new PlcHead
            {
                PlcKey = Guid.NewGuid(),
                AtrKey = placeDTO.AtrKey,
                DesKey = placeDTO.DesKey,
                Description = placeDTO.Description,
                Latitude = placeDTO.Latitude,
                Longitude = placeDTO.Longitude,
                PicKey = placeDTO.PicKey,
                PlcName = placeDTO.PlaceName,
            };

            try
            {
                _context.PlcHeads.Add(place);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in create Place");
            }
            var cPlaceDTO = new PlaceDTO
            {
                AtrKey = place.AtrKey,
                Description = place.Description,
                DesKey = place.DesKey,
                Latitude = place.Latitude,
                Longitude = place.Longitude,
                PicKey = place.PicKey,
                PlaceKey = place.PlcKey,
                PlaceName = place.PlcName,
            };
            return cPlaceDTO;
        }

        [HttpPut]
        public async Task<UPlaceDTO> Update([FromBody] UPlaceDTO placeDTO)
        {
            if (placeDTO == null)
            {
                throw new Exception(nameof(Create) + " Model is null or invalid");
            }

            var destination = _context.DesHeads.Where(item => item.DesKey == placeDTO.DesKey)
                                               .AsNoTracking()
                                               .FirstOrDefault()
                                               ?? throw new ArgumentNullException();

            var attraction = _context.AtrHeads.Where(item => item.AtrKey == placeDTO.AtrKey)
                                              .AsNoTracking()
                                              .FirstOrDefault()
                                              ?? throw new ArgumentNullException();

            var place = _context.PlcHeads.Where(item => item.PlcKey == placeDTO.PlaceKey)
                                              .AsNoTracking()
                                              .FirstOrDefault()
                                              ?? throw new ArgumentNullException();

            var newPlace = new PlcHead
            {
                PlcKey = placeDTO.PlaceKey,
                AtrKey = placeDTO.AtrKey,
                DesKey = placeDTO.DesKey,
                Description = placeDTO.Description,
                Latitude = placeDTO.Latitude,
                Longitude = placeDTO.Longitude,
                PicKey = placeDTO.PicKey,
                PlcName = placeDTO.PlaceName,
            };

            try
            {
                _context.PlcHeads.Update(newPlace);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in update Place");
            }

            return placeDTO;
        }

        [HttpDelete]
        public async Task Delete([FromBody] Guid? placeKey)
        {
            if (placeKey == null)
            {
                throw new Exception(nameof(Delete) + "Place key is null or invalid!");
            }

            var existPlace = await _context.PlcHeads.FirstOrDefaultAsync(plc => plc.PlcKey == placeKey);

            if (existPlace == null)
            {
                throw new Exception(nameof(Delete) + "Place is not found!");
            }

            try
            {
                _context.PlcHeads.Remove(existPlace);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Delete Place", ex);
            }
        }

        [HttpGet("InitPlaceData")]
        public async Task<ActionResult<List<DropdownPlaceDTO>>> GetInitDropdownData()
        {
            try
            {
                var result = await _context.DesHeads.AsNoTracking()
                     .Join(
                         _context.PlcHeads.AsNoTracking(),
                         des => des.DesKey,
                         plc => plc.DesKey,
                        (des, plc) => new
                        {
                            des.DesKey,
                            des.DesName,
                            plc.PlcKey,
                            plc.PlcName
                        }
                     )
                     .GroupBy(value => value.DesKey)
                     .Select(gr => new DropdownPlaceDTO
                     {
                         Destination = new DesKeyValueDTO
                         {
                             DesKey = gr.Key,
                             DesName = gr.First().DesName,
                         },
                         Places = gr.Select(item => new PlaceKeyValueDTO
                         {
                             PlcKey = item.PlcKey,
                             PlcName = item.PlcName
                         }).ToList()

                     })
                     .ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in get init dropdown place", ex);
            }
        }

    }
}
