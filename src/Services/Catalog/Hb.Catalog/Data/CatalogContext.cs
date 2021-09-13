using Hb.Catalog.Data.Interfaces;
using Hb.Catalog.Entities;
using Hb.Catalog.Settings;
using MongoDB.Driver;

namespace Hb.Catalog.Data
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            Products = database.GetCollection<Product>(settings.CollectionName);
            CatalogContextSeed.SeedData(Products);
        }

        public IMongoCollection<Product> Products { get; }

    }
}
