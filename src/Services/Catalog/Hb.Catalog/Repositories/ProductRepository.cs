using Hb.Catalog.Data.Interfaces;
using Hb.Catalog.Entities;
using Hb.Catalog.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hb.Catalog.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;
        private readonly IDistributedCache _redisCache;

        public ProductRepository(ICatalogContext context
            , IDistributedCache redisCache)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
        }
        
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context
                            .Products
                            .Find(p => true)
                            .ToListAsync();
        }
        public async Task<Product> GetProduct(string id)
        {
            var productCache = await _redisCache.GetStringAsync(id);

            if (String.IsNullOrEmpty(productCache))
            {
                var product = await _context
                                        .Products
                                        .Find(p => p.Id == id)
                                        .FirstOrDefaultAsync();

                if(product != null)
                {
                    await _redisCache.SetStringAsync(id
                                                , JsonConvert.SerializeObject(product)
                                                , new DistributedCacheEntryOptions
                                                {
                                                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                                                });
                }                

                return product;
            }

            return JsonConvert.DeserializeObject<Product>(productCache);
        }
        //public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        //{
        //    FilterDefinition<Product> filter = Builders<Product>
        //                    .Filter
        //                    .Eq(p => p.CategoryId, categoryName);

        //    return await _context
        //                    .Products
        //                    .Find(filter)
        //                    .ToListAsync();
        //}
        //public async Task<IEnumerable<Product>> GetProductByName(string name)
        //{
        //    FilterDefinition<Product> filter = Builders<Product>
        //                    .Filter
        //                    .ElemMatch(p => p.Name, name);

        //    return await _context
        //                    .Products
        //                    .Find(filter)
        //                    .ToListAsync();
        //}

        public async Task Create(Product product)
        {
            await _context.Products.InsertOneAsync(product);

            await _redisCache.SetStringAsync(product.Id
                                                 , JsonConvert.SerializeObject(product)
                                                 , new DistributedCacheEntryOptions
                                                 {
                                                     AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                                                 });
        }
        public async Task<bool> Update(Product product)
        {
            var updateResult = await _context
                                        .Products
                                        .ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);

            if (updateResult.IsAcknowledged && updateResult.ModifiedCount > 0)
            {
                await _redisCache.SetStringAsync(product.Id
                                                , JsonConvert.SerializeObject(product)
                                                , new DistributedCacheEntryOptions
                                                {
                                                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                                                });

                return true;
            }

            return false;
        }
        public async Task<bool> Delete(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                                .Products
                                                .DeleteOneAsync(filter);

            if (deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0)
            {
                await _redisCache.RemoveAsync(id);

                return true;
            }

            return false;
        }
    }
}
