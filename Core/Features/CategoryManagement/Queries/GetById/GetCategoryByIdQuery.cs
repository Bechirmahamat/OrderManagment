using Core.Responses;
using MediatR;

namespace Core.Features.CategoryManagement.Queries.GetById
{
    public record GetCategoryByIdQuery(Guid id) : IRequest<GenericResponse>
    {
    }
}
