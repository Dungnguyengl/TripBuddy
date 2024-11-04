using CommonService.Services;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenController(IInternalService internalService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<Response<LoginDto>> Login([FromBody] LoginCommand command)
        {
            var res = await internalService.PostAsync<LoginDto>(CommonService.Constants.ServiceType.Authentication, command, "login");
            return res;
        }

        [HttpPost("register")]
        public async Task<Response<RegisterDto>> Register([FromBody] RegisterCommand command)
        {
            var res = await internalService.PostAsync<RegisterDto>(CommonService.Constants.ServiceType.Authentication, command, "register");
            return res;
        }

        [HttpPost("refresh")]
        public async Task<Response<LoginDto>> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            var res = await internalService.PostAsync<LoginDto>(CommonService.Constants.ServiceType.Authentication, command, "refresh");
            return res;
        }
    }
}
