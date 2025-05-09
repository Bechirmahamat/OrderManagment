namespace Domain.Entities
{
    public class Order : BaseEntity
    {
        public string Name { get; set; }
        public double TotalPrice { get; set; }
        public string UserId { get; set; }
        public User? User { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public Order(string name, double totalPrice, string userId)
        {
            Name = name;
            TotalPrice = totalPrice;
            UserId = userId;
        }
    }
}
