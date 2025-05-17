using Core.Responses;
using MediatR;

namespace Core.Features.CompanyManagement.Queries.GetById
{
    public record GetCompnayByIdQuery(Guid id) : IRequest<GenericResponse>
    {
    }
}
