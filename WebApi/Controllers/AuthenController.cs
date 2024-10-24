using CommonService.Services;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AuthenController(IInternalService internalService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<LoginDto> Login([FromBody] LoginCommand command)
        {
            var res = await internalService.PostAsync<LoginDto>(CommonService.Constants.ServiceType.Authentication, command, "login");
            if (res.Code != System.Net.HttpStatusCode.OK)
                throw new Exception();
            return res.Content;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            var res = await internalService.PostAsync<LoginDto>(CommonService.Constants.ServiceType.Authentication, command, "register");
            if (res.Code != System.Net.HttpStatusCode.OK)
                throw new (res.Code.ToString());
            return Ok();
        }

        [HttpPost("refresh")]
        public async Task<Response<LoginDto>> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            var res = await internalService.PostAsync<LoginDto>(CommonService.Constants.ServiceType.Authentication, command, "refresh");
            return res;
        }
    }
}
