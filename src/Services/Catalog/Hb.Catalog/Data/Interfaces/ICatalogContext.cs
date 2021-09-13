using Hb.Catalog.Entities;
using MongoDB.Driver;

namespace Hb.Catalog.Data.Interfaces
{
    public interface ICatalogContext
    {
        IMongoCollection<Product> Products { get; }
    }
}
