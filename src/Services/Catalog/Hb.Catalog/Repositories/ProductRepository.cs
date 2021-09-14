using AutoMapper;
using EventBusRabbitMQ.Core;
using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Producer;
using Hb.Catalog.Data.Interfaces;
using Hb.Catalog.Entities;
using Hb.Catalog.Repositories.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hb.Catalog.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;
        private readonly IMapper _mapper;
        private readonly EventBusRabbitMQProducer _eventBusRabbitMQProducer;

        public ProductRepository(ICatalogContext context
            , IMapper mapper
            , EventBusRabbitMQProducer eventBusRabbitMQProducer)
        {
            _context = context;
            _mapper = mapper;
            _eventBusRabbitMQProducer = eventBusRabbitMQProducer;
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
            //var productCache = await _redisCache.GetStringAsync(id);

            //if (String.IsNullOrEmpty(productCache))
            //{
            var product = await _context
                                    .Products
                                    .Find(p => p.Id == id)
                                    .FirstOrDefaultAsync();

            ProductCacheCreateEvent eventMessage = _mapper.Map<ProductCacheCreateEvent>(product);

            try
            {
                _eventBusRabbitMQProducer.Publish(EventBusConstants.ProductCacheCreateQueue, eventMessage);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "ERROR Publishing integration event: {EventId} from {AppName}", eventMessage.Id, "ProductRepository");
                //throw;
            }

            //await _redisCache.SetStringAsync(id
            //                                , JsonConvert.SerializeObject(product)
            //                                , new DistributedCacheEntryOptions
            //                                {
            //                                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            //                                });

            return product;
            //}

            //return JsonConvert.DeserializeObject<Product>(productCache);
        }
        public async Task<IEnumerable<Product>> GetProductByCategoryId(string categoryName)
        {
            FilterDefinition<Product> filter = Builders<Product>
                            .Filter
                            .Eq(p => p.CategoryId, categoryName);

            return await _context
                            .Products
                            .Find(filter)
                            .ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>
                            .Filter
                            .ElemMatch(p => p.Name, name);

            return await _context
                            .Products
                            .Find(filter)
                            .ToListAsync();
        }

        public async Task Create(Product product)
        {
            await _context
                    .Products
                    .InsertOneAsync(product);
        }
        public async Task<bool> Update(Product product)
        {
            var updateResult = await _context
                                        .Products
                                        .ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }
        public async Task<bool> Delete(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>
                            .Filter
                            .Eq(m => m.Id, id);

            DeleteResult deleteResult = await _context
                                                .Products
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }
    }
}
