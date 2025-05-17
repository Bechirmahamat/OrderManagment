using Core.Responses;
using MediatR;

namespace Core.Features.ProductManagement.Queries.GetById
{
    public record GetProductByIdQuery(Guid id) : IRequest<GenericResponse>
    {
    }
}
