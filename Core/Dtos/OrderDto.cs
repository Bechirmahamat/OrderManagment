using Domain.Entities;

namespace Core.Dtos
{
    public class OrderDto : BaseDto
    {
        public string Name { get; set; }
        public double TotalPrice { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }
}
