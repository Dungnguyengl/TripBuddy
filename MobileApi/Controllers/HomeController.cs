
using Microsoft.AspNetCore.Mvc;

namespace MobileApi.Controllers
{
    [Route("api/[Controller]")]
    public class HomeController
    {
        [HttpGet]
        public async Task <Test> Test()
        {
            return new() {Message = "sdfasdfa"};
        }
    }
}
