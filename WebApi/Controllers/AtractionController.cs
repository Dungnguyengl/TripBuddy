using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AtractionController : ControllerBase
    {
        [HttpGet("search")]
        public IActionResult GetAllAtraction()
        {
            return Ok();
        }
    }
}
