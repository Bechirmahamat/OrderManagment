namespace Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public double Price { get; set; }
        public double TotalPrice { get; set; }
        public int Quantity { get; set; }
        public Guid CompanyId { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }

        public Order? Order { get; set; }
        public Product? Product { get; set; }
        public Company? Company { get; set; }
    }
}
