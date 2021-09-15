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
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            //Products = database.GetCollection<Product>(settings.CollectionName);
            //CatalogContextSeed.SeedData(Products);
        }

        public IMongoCollection<Product> Products { get; }

    }
}
