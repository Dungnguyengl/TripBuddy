using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotService.Model;

namespace SpotService.Controllers.Destination
{
    [Route("api/[controller]")]
    public class DestinationController(SpotDbContext context) : ControllerBase
    {
        private readonly SpotDbContext _context = context;

        //[HttpGet]
        //public Task<List<DestinationDTO>> Get()
        //{
        //    var query = from head in _context.DesHeads.AsNoTracking()
        //                select new DestinationDTO
        //                {
        //                    DesKey = head.DesKey,
        //                    DesDescription = head.Description ?? string.Empty,
        //                    DesName = head.DesName ?? string.Empty,
        //                    DesPic = head.PicKey,
        //                    AtrKey = head.AtrKey,
        //                };

        //    return Task.FromResult(query.ToList());
        //}

        [HttpGet("Details")]
        public ActionResult<DestinationDTO> Details([FromQuery]Guid key)
        {
            var query = from head in _context.DesHeads.AsNoTracking()
                        where head.DesKey == key
                        select new DestinationDTO
                        {
                            DesKey = head.DesKey,
                            DesDescription = head.Description ?? string.Empty,
                            DesName = head.DesName ?? string.Empty,
                            DesPic = head.PicKey,
                            AtrKey = head.AtrKey,
                        };
            if (query.Any())
            {
                return Ok(query.FirstOrDefault());
            }
            else return NotFound();
        }

        [HttpGet("Search")]
        public Task<List<DestinationDTO>> Search([FromQuery]string search)
        {
            var searchTerm = search.ToUpper();
            var attractions = _context.AtrHeads.Where(item => item.Contitnent.ToUpper().Contains(searchTerm)
                                                      || item.SubContitnent.ToUpper().Contains(searchTerm)
                                                      || item.Country.ToUpper().Contains(searchTerm))
                                               .AsNoTracking().Select(item => item.AtrKey).ToList();

            var desHead = _context.DesHeads.Where(item => item.DesName.ToUpper().Contains(searchTerm)
                                                  || attractions.Contains(item.AtrKey))
                                           .AsNoTracking();

            var query = from head in desHead
                        select new DestinationDTO
                        {
                            DesKey = head.DesKey,
                            DesDescription = head.Description ?? string.Empty,
                            DesName = head.DesName ?? string.Empty,
                            DesPic = head.PicKey,
                            AtrKey = head.AtrKey,
                        };

            var destinations = query.ToList();
            return Task.FromResult(destinations);
        }

        [HttpGet("Keys")]
        public ActionResult<DesKeyValueDTO> GetDesKeyValue()
        {
            var keyValuePairs = from des in _context.DesHeads.AsNoTracking()
                                select new DesKeyValueDTO
                                {
                                    DesKey = des.DesKey,
                                    DesName = des.DesName,
                                };

            return Ok(keyValuePairs);
        }


        [HttpPost]
        public async Task<DesHead> Create([FromBody] CDestinationDTO destinationDTO)
        {
            if (destinationDTO == null)
            {
                throw new Exception(nameof(Create) + " Model is null or invalid");
            }

            var attraction = _context.AtrHeads.Where(item => item.AtrKey == destinationDTO.AtrKey)
                                              .AsNoTracking()
                                              .FirstOrDefault()
                                              ?? throw new ArgumentNullException();

            var destination = new DesHead
            {
                DesKey = Guid.NewGuid(),
                AtrKey = destinationDTO.AtrKey,
                Description = destinationDTO.DesDescription ?? string.Empty,
                NoVisted = 0,
                DesName = destinationDTO.DesName ?? string.Empty,
                AtrContents = new List<AtrContent>(),
                PicKey = null,
                PlcHeads = new List<PlcHead>(),
            };

            try
            {
                _context.DesHeads.Add(destination);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in create Destination");
            }

            return destination;
        }

        [HttpPut]
        public async Task<DesHead> Update([FromBody] UDestinationDto destinationDTO)
        {
            if (destinationDTO == null)
            {
                throw new Exception(nameof(Create) + " Model is null or invalid");
            }

            var destination = _context.DesHeads.Where(item => item.DesKey == destinationDTO.DesKey)
                                               .AsNoTracking()
                                               .FirstOrDefault()
                                               ?? throw new ArgumentNullException();

            var attraction = _context.AtrHeads.Where(item => item.AtrKey == destinationDTO.AtrKey)
                                              .AsNoTracking()
                                              .FirstOrDefault()
                                              ?? throw new ArgumentNullException();

            var newDestination = new DesHead
            {
                DesKey = destinationDTO.DesKey,
                AtrKey = destinationDTO.AtrKey,
                Description = destinationDTO.DesDescription ?? string.Empty,
                NoVisted = destination.NoVisted,
                DesName = destinationDTO.DesName ?? string.Empty,
                AtrContents = destination.AtrContents,
                PicKey = destination.PicKey,
                PlcHeads = destination.PlcHeads,
            };

            try
            {
                _context.DesHeads.Update(newDestination);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in update Destination");
            }

            return destination;
        }

    }
}
