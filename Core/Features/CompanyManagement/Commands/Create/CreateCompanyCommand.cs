using Core.Responses;
using MediatR;

namespace Core.Features.CompanyManagement.Commands.Create
{
    public record CreateCompanyCommand(string Name, string Description, string ManagerId) : IRequest<GenericResponse>
    {
    }
}
