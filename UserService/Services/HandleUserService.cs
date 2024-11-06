using UserService.Models;

namespace UserService.Services
{
    public class HandleUserService(UserServiceDbContext context, ILogger<HandleUserService> logger)
    {
        private readonly UserServiceDbContext _context = context;
        private readonly ILogger<HandleUserService> _logger = logger;
    }
}
