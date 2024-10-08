using CommonService.Constants;
using CommonService.Services;
using Microsoft.AspNetCore.Mvc;

namespace MobileApi.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        private IInternalService _authenService;

        public TestController(IInternalService authenService)
        {
            _authenService = authenService;
        }

        [HttpGet]
        public async Task<Test> Test ()
        {
            var res = await _authenService.GetAsync<Test>(ServiceType.Authentication, new { Name = "Dung" }, "home");
            return res.Content;
        }
    }

    public class Test
    {
        public string Message;
    }
}