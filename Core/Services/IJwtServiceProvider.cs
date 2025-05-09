using Domain.Entities;

namespace Core.Services
{
    public interface IJwtServiceProvider
    {
        string GenerateToken(User user);
        string GenerateRefreshToken();

    }
}
