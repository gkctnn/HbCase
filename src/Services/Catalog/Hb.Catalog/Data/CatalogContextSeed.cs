using Hb.Catalog.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Hb.Catalog.Data
{
    public class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productCollection)
        {
            bool existProduct = productCollection.Find(p => true).Any();

            if (!existProduct)
            {
                productCollection.InsertManyAsync(GetConfigureProducts());
            }
        }

        private static IEnumerable<Product> GetConfigureProducts()
        {
            return new List<Product>()
            {
                new Product()
                {
                    CategoryId = "602d2149e773f2a3990b47f8",
                    Name = "Product 1",
                    Description = "Product Description 1",
                    Price = 10.0m,
                    Currency = "TL"
                },
                new Product()
                {
                    CategoryId = "602d2149e773f2a3990b47f9",
                    Name = "Product 2",
                    Description = "Product Description 2",
                    Price = 15.0m,
                    Currency = "TL"
                },
                new Product()
                {
                    CategoryId = "602d2149e773f2a3990b47fa",
                    Name = "Product 3",
                    Description = "Product Description 3",
                    Price = 7.0m,
                    Currency = "TL"
                }
            };
        }
    }
}
