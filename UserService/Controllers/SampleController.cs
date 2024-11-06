using Microsoft.AspNetCore.Mvc;
using UserService.Services;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SampleController(HandleUserService service) : ControllerBase
    {
        private readonly HandleUserService _service = service;

        [HttpGet]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
