using Hb.Domain.Entities;
using MongoDB.Driver;

namespace Hb.Catalog.Infrastructures.Data.Interfaces
{
    public interface ICatalogContext
    {
        IMongoCollection<Product> Products { get; }
        IMongoCollection<Category> Categories { get; }
    }
}
