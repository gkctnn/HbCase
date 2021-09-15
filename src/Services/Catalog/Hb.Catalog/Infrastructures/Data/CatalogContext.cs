using Hb.Catalog.Infrastructures.Data.Interfaces;
using Hb.Catalog.Infrastructures.Settings;
using Hb.Domain.Entities;
using MongoDB.Driver;

namespace Hb.Catalog.Infrastructures.Data
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IDatabaseSettings settings)
        {

        }

        public IMongoCollection<Product> Products { get; }
        public IMongoCollection<Category> Categories { get; }

    }
}
