using System.ComponentModel.DataAnnotations;

namespace Api.Dtos
{

    public record RegisterRequest([Required] string FirstName, [Required] string LastName, [Required][EmailAddress] string Email, [Required] string Password);
    public record LoginRequest([Required][EmailAddress] string Email, [Required] string Password);
    public record RefreshTokenRequest([Required] string Token, [Required] string RefreshToken);

}
