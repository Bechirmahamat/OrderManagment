namespace Core.Dtos
{
    public class OrderItemDto : BaseDto
    {

        public double Price { get; set; }
        public double TotalPrice { get; set; }
        public int Quantity { get; set; }
        public Guid CompanyId { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public OrderDto? Order { get; set; }
        public ProductDto? Product { get; set; }
    }
}
