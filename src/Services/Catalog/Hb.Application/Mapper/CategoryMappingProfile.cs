using AutoMapper;
using Hb.Application.Commands.CategoryCreate;
using Hb.Application.Responses;
using Hb.Domain.Entities;

namespace Hb.Application.Mapper
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<Category, CategoryCreateCommand>().ReverseMap();
            CreateMap<Category, CategoryResponse>().ReverseMap();
        }
    }
}
