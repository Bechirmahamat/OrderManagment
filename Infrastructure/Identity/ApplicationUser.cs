using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? TokenCreated { get; set; } = DateTime.UtcNow;
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
