using AutoMapper;
using Core.Dtos;
using Domain.Entities;

namespace Core.Mappings
{
    public class mappingProfiles : Profile
    {
        public mappingProfiles()
        {
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<CategoryDto, ProductCategory>().ReverseMap();
            CreateMap<OrderDto, Order>();
            CreateMap<OrderItemDto, OrderItem>();
            CreateMap<CompanyDto, Company>();
        }
    }
}
