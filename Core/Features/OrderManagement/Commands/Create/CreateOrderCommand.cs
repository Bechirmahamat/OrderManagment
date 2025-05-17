using Core.Responses;
using MediatR;

namespace Core.Features.OrderManagement.Commands.Create
{
    public class CreateOrderCommand : IRequest<GenericResponse>

    {
        public string UserId { get; set; } = string.Empty;
        public Guid CompanyId { get; set; } // Add this missing foreign key
        public string OrderStatus { get; set; } = "Pending"; // Default status

        public List<OrderItemRequest> OrderItems { get; set; }

    }
    public class OrderItemRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}