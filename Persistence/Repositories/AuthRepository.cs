using Core.Interface;
using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Persistence.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<AuthRepository> logger;

        public AuthRepository(UserManager<ApplicationUser> userManager, ILogger<AuthRepository> logger)
        {
            this.userManager = userManager;

            this.logger = logger;
        }
        public async Task<User?> Login(string email, string password)
        {
            try
            {

                var user = await userManager.Users.FirstOrDefaultAsync(x => x.Email == email);
                if (user == null)
                {
                    return new User();
                }
                var result = await userManager.CheckPasswordAsync(user, password);

                if (result)
                {
                    return new User
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email ?? user.UserName
                    };

                }
                else
                {
                    logger.LogWarning("Invalid password");
                    return null;
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error logging in user");
                return new User();
            }
        }

        public async Task<bool> RegisterUser(User user, string password)
        {
            try
            {
                var applicationUser = new ApplicationUser
                {
                    UserName = user.Email,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,

                };

                var data = await userManager.CreateAsync(applicationUser, password);
                if (data.Succeeded)
                {
                    // ✅ Add role
                    var roleResult = await userManager.AddToRoleAsync(applicationUser, "User");

                    if (user.Email == "admin@gmail.com")
                    {
                        roleResult = await userManager.AddToRoleAsync(applicationUser, "Admin");

                    }
                    if (!roleResult.Succeeded)
                    {
                        logger.LogWarning("Role assignment failed: {Errors}", string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                        return false;
                    }

                    return true;

                }
                else
                {
                    logger.LogWarning("User creation failed", data);
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error logging in user");
                return false;
            }
        }
        public async Task<bool> UserExists(string email)
        {
            try
            {
                var user = await userManager.Users.FirstOrDefaultAsync(x => x.Email == email);
                if (user != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error logging in user");
                return false;
            }
        }
        public async Task SaveRefreshToken(string userId, string refreshToken)
        {
            try
            {
                var user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
                if (user != null)
                {
                    user.RefreshToken = refreshToken;
                    // Set the refresh token expiry time to 7 days from now
                    user.TokenCreated = DateTime.UtcNow;
                    user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                    await userManager.UpdateAsync(user);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error saving refresh token");
            }
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            try
            {
                var user = await userManager.Users.FirstOrDefaultAsync(x => x.Email == email);
                return new User { Email = email, FirstName = user.FirstName, LastName = user.LastName, Id = user.Id };

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error logging in user");
                return null;
            }

        }
        public async Task<User?> GetUserById(string id)
        {
            try
            {
                var user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
                if (user == null) return null;
                return new User { Email = user.Email, FirstName = user.FirstName, LastName = user.LastName, Id = user.Id };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error logging in user");
                return null;
            }
        }
        public async Task<bool> AddUserRole(string userId, string role)
        {
            try
            {
                var user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
                if (user != null)
                {
                    var result = await userManager.AddToRoleAsync(user, role);
                    return result.Succeeded;
                }
                return false;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error adding user role");
                return false;
            }
        }
        public Task<bool> ChangePassword(string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ConfirmEmail(string email, string token)
        {
            throw new NotImplementedException();
        }
        public Task<bool> ResetPassword(string email, string token, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendEmailConfirmation(string email)
        {
            throw new NotImplementedException();
        }


    }
}
