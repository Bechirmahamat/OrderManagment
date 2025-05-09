namespace Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int StockCount { get; set; }
        public double Price { get; set; }

        public Guid CompanyId { get; set; }
        public Guid CategoryId { get; set; }
        public Company? Company { get; set; }
        public ProductCategory Category { get; set; }


    }

}
