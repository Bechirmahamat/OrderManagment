using Core.Responses;
using MediatR;

namespace Core.Features.ProductManagement.Commands.Create
{
    public record CreateProductCommand(string Name, string Description, double Price, int StockCount, Guid CategoryId, string ManagerId) : IRequest<GenericResponse>
    {

    }
}