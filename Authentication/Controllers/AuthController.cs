using Authentication.Model;
using Authentication.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController(UserManager<AuthenticationUser> userManager, SignInManager<AuthenticationUser> signInManager, IConfiguration configuration, TokenService tokenService) : ControllerBase
    {
        private readonly UserManager<AuthenticationUser> _userManager = userManager;
        private readonly SignInManager<AuthenticationUser> _signInManager = signInManager;
        private readonly IConfiguration _configuration = configuration;
        private readonly TokenService _tokenService = tokenService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand model)
        {
            var user = new AuthenticationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok(new { Message = "User registered successfully" });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email) ?? throw new Exception();
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (result.Succeeded)
            {
                var token = await _tokenService.GenerateJwtTokenAsync(user);
                var refreshToken = _tokenService.GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpire = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:Refresh_Expire"]));

                await _userManager.UpdateAsync(user);
                return Ok(new
                {
                    AccessToken = token,
                    RefreshToken = refreshToken
                });
            }

            return Unauthorized();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefresCommand command)
        {
            var principal = _tokenService.GetPrincipalFromToken(command.AccessToken);
            var user = await _userManager.FindByIdAsync(principal.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value ?? "")
                .ConfigureAwait(true);
            if (user == null || user.RefreshToken != command.RefreshToken || user.RefreshTokenExpire <= DateTime.Now)
            {
                return BadRequest("Invalid client request");
            }

            var newAccessToken = await _tokenService.GenerateJwtTokenAsync(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpire = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:Refresh_Expire"]));

            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        public class RegisterCommand
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class LoginCommand
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public class RefresCommand
        {
            public string AccessToken { get; set; }
            public string RefreshToken { get; set; }
        }
    }
}