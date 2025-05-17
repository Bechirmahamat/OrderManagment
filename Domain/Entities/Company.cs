namespace Domain.Entities
{
    public class Company : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ManagerId { get; set; }
        public List<Product>? Products { get; set; }
        public List<Order>? Orders { get; set; }


    }
}
