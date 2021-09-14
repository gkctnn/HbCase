using AutoMapper;
using EventBusRabbitMQ.Events;
using Hb.Catalog.Entities;

namespace Hb.Catalog.Mapping
{
    public class CatalogMapping : Profile
    {
        public CatalogMapping()
        {
            CreateMap<ProductCacheCreateEvent, Product>().ReverseMap();
        }
    }
}
