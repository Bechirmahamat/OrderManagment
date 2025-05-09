using Domain.Entities;

namespace Core.Dtos
{
    public class ProductDto : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int StockCount { get; set; }
        public double Price { get; set; }
        public Guid CompanyId { get; set; }
        public Guid CategoryId { get; set; }
        public Company? Company { get; set; }
        public CategoryDto Category { get; set; }
    }
}
