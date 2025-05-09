using Core.Responses;
using MediatR;

namespace Core.Features.AuthManagement.Commands.Login
{
    public record LoginCommand(string email, string password) : IRequest<GenericResponse>
    {
    }
}
