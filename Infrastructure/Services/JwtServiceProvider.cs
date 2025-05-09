using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Core.Services;
using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services
{
    public class JwtServiceProvider : IJwtServiceProvider
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;

        public JwtServiceProvider(UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            this.userManager = userManager;
            this.config = config;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public string GenerateToken(User user)
        {
            var currentUser = userManager.Users.FirstOrDefault(u => u.Email == user.Email);
            var userRole = userManager.GetRolesAsync(currentUser).Result;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Email, user.Email),

            };
            var roles = userRole.Select(r => new Claim(ClaimTypes.Role, r)).ToList();
            claims.AddRange(roles);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtConfig:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: config["JwtConfig:Issuer"],
                audience: config["JwtConfig:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
