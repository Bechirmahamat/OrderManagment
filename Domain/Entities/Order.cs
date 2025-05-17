namespace Domain.Entities
{
    public class Order : BaseEntity
    {
        public string OrderStatus { get; set; }
        public double TotalPrice => OrderItems.Sum(oi => oi.Price * oi.Quantity);
        public string UserId { get; set; }
        public Guid CompanyId { get; set; }  // Add this missing foreign key

        public Company Company { get; set; }  // Navigation to Company
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public Order(string userId, Guid companyId)
        {
            OrderStatus = "Pending"; // Default status
            UserId = userId;
            CompanyId = companyId;
        }
    }
}
