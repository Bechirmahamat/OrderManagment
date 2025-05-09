using Core.Responses;
using MediatR;

namespace Core.Features.AuthManagement.Commands.Register
{
    public record RegisterUserCommand(string FirstName, string LastName, string Email, string Password) : IRequest<GenericResponse>
    {
    }
}
