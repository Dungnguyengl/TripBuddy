using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotService.Model;

namespace SpotService.Controllers.Place
{
    [Route("api/[controller]")]
    public class PlaceController(SpotDbContext context) : ControllerBase
    {
        private readonly SpotDbContext _context = context;

        [HttpGet]
        public Task<List<PlaceDTO>> Get()
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

            return Task.FromResult(query.ToList());
        }

        [HttpGet("details/{key}")]
        public ActionResult<PlaceDTO> Details(string key)
        {
            var query = from head in _context.PlcHeads.AsNoTracking()
                        where head.AtrKey.ToString() == key
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

            var place = query.FirstOrDefault();
            if (place == null)
            {
                return NotFound();
            }
            else
                return place;
        }

        [HttpGet("{search}")]
        public Task<List<PlaceDTO>> Search(string search)
        {
            var attractions = _context.AtrHeads.Where(item => item.Contitnent.ToUpper().Contains(search.ToUpper())
                                                      || item.SubContitnent.ToUpper().Contains(search.ToUpper())
                                                      || item.Country.ToUpper().Contains(search.ToUpper()))
                                               .AsNoTracking().Select(item => item.AtrKey).ToList();

            var desHead = _context.DesHeads.Where(item => item.DesName.ToUpper().Contains(search.ToUpper()))
                                           .AsNoTracking().Select(item => item.DesKey).ToList();

            var plcHead = _context.PlcHeads.Where(item => item.PlcName.ToUpper().Contains(search.ToUpper())
                                                       || attractions.Contains(item.AtrKey)
                                                       || desHead.Contains(item.DesKey));

            var query = from head in plcHead

                        select new PlaceDTO
                        {
                            AtrKey = head.AtrKey,
                            DesKey = head.DesKey,
                            Description = head.Description,
                            Latitude = head.Latitude,
                            Longitude = head.Longitude,
                            PicKey = head.PicKey,
                            PlaceKey = head.PlcKey,
                            PlaceName = head.PlcName,
                        };

            return Task.FromResult(query.ToList());
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
                                              ?? throw new
                                              Exception(nameof(Create) + " Attraction is invalid");

            var destination = _context.DesHeads.Where(item => item.DesKey == placeDTO.DesKey)
                                              .AsNoTracking()
                                              .FirstOrDefault()
                                              ?? throw new
                                              Exception(nameof(Create) + " Destination is invalid");

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
        public async Task<PlaceDTO> Update([FromBody] PlaceDTO placeDTO)
        {
            if (placeDTO == null)
            {
                throw new Exception(nameof(Create) + " Model is null or invalid");
            }

            var destination = _context.DesHeads.Where(item => item.DesKey == placeDTO.DesKey)
                                               .AsNoTracking()
                                               .FirstOrDefault()
                                               ?? throw new Exception(nameof(Update) + " Destination is invalid");

            var attraction = _context.AtrHeads.Where(item => item.AtrKey == placeDTO.AtrKey)
                                              .AsNoTracking()
                                              .FirstOrDefault()
                                              ?? throw new
                                              Exception(nameof(Update) + " Attraction is invalid");

            var place = _context.PlcHeads.Where(item => item.PlcKey == placeDTO.PlaceKey)
                                              .AsNoTracking()
                                              .FirstOrDefault()
                                              ?? throw new
                                              Exception(nameof(Update) + " Place is invalid");

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


    }
}
