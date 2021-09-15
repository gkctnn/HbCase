using AutoMapper;
using Hb.Application.Commands.ProductCreate;
using Hb.Application.Responses;
using Hb.Domain.Entities;

namespace Hb.Application.Mapper
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductCreateCommand>().ReverseMap();
            CreateMap<Product, ProductResponse>().ReverseMap();
        }
    }
}
