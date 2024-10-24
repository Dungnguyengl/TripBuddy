namespace WebApi.Dtos
{
    public class LoginCommand
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }

    public class RegisterCommand
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class RefreshTokenCommand
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
