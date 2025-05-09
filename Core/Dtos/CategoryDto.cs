namespace Core.Dtos
{
    public class CategoryDto
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public List<ProductDto>? Products { get; set; }
    }
}
