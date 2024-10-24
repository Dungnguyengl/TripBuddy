using Authentication.Services;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController(TokenService tokenService) : ControllerBase
    {
        private readonly TokenService _tokenService = tokenService;

        [HttpPost("isValid")]
        public IActionResult CheckTokenValid(CheckTokenValidCommand command)
        {
            var isValidToken = _tokenService.IsValidToken(command.AccessToken);
            var message = new { Message = isValidToken ? "Valid" : "Invalid" };

            return isValidToken ? Ok(message) : BadRequest(message);
        }
    }

    public class CheckTokenValidCommand
    {
        public string AccessToken { get; set; }
    }
}
