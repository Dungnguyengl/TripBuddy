using Microsoft.AspNetCore.Mvc;

namespace Authentication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Hello([FromQuery] string name)
        {
            return Ok(new { Message = $"Hello {name} from home with love!" });
        }
    }
}
