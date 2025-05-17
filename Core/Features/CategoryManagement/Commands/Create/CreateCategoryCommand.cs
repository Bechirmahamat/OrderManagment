using Core.Responses;
using MediatR;

namespace Core.Features.CategoryManagement.Commands.Create
{
    public record CreateCategoryCommand(string Name, string Desciption) : IRequest<GenericResponse>
    {
    }
}
