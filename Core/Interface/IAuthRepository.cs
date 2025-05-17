using Domain.Entities;

namespace Core.Interface
{
    public interface IAuthRepository
    {
        Task<bool> RegisterUser(User user, string password);
        Task<bool> UserExists(string email);
        Task<User?> GetUserByEmail(string email);
        Task<User?> GetUserById(string id);
        Task SaveRefreshToken(string userId, string refreshToken);
        Task<User?> Login(string email, string password);
        Task<bool> ConfirmEmail(string email, string token);
        Task<bool> SendEmailConfirmation(string email);
        Task<bool> ResetPassword(string email, string token, string newPassword);
        Task<bool> ChangePassword(string oldPassword, string newPassword);
        Task<bool> AddUserRole(string userId, string role);
    }
}
