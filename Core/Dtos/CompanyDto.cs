namespace Core.Dtos
{
    public class CompanyDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ManagerId { get; set; }
        public UserDto? Manager { get; set; }
        public List<ProductDto>? Products { get; set; }
        public List<OrderDto>? Orders { get; set; }

    }
}
