using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Model
{
    public class AuthenticationUser : IdentityUser<Guid>
    {
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpire { get; set; }
    }

    public class AuthenticationRole : IdentityRole<Guid>
    {

    }

    public class AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options) : IdentityDbContext<AuthenticationUser, AuthenticationRole, Guid>(options)
    {
        public void ApplyMigrations()
        {
            Database.Migrate();
        }
    }
}
