using Hb.Catalog.Infrastructures.Settings;
using Hb.Domain.Entities.Base;
using Hb.Domain.Repositories.Base;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Hb.Catalog.Infrastructures.Repositories.Base
{
    public abstract class Repository<T> : IRepository<T> where T : Entity, new()
    {
        private const string REDIS_CACHE_BY_ID = "Hb.Get.{0}-{1}";

        protected readonly IMongoCollection<T> Collection;
        private readonly IDatabaseSettings settings;

        private readonly IDistributedCache _redisCache;

        protected Repository(IOptions<IDatabaseSettings> options
            , IDistributedCache redisCache)
        {
            this.settings = options.Value;
            var client = new MongoClient(this.settings.ConnectionString);
            var db = client.GetDatabase(this.settings.DatabaseName);
            this.Collection = db.GetCollection<T>(typeof(T).Name.ToLowerInvariant());

            _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
        }

        public virtual IQueryable<T> Get(Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null
                    ? Collection.AsQueryable()
                    : Collection.AsQueryable().Where(predicate);
        }
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await Collection
                            .Find(predicate)
                            .ToListAsync();
        }
        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await Collection
                            .Find(predicate)
                            .FirstOrDefaultAsync();
        }
        public async Task<T> GetByIdAsync(string id, int expireTime = 60)
        {
            var productCache = await _redisCache.GetStringAsync(string.Format(REDIS_CACHE_BY_ID, typeof(T).Name, id));

            if (String.IsNullOrEmpty(productCache))
            {
                var product = await Collection
                                        .Find(p => p.Id == id)
                                        .FirstOrDefaultAsync();

                if (product != null)
                {
                    await _redisCache.SetStringAsync(string.Format(REDIS_CACHE_BY_ID, typeof(T).Name, id)
                                                , JsonConvert.SerializeObject(product)
                                                , new DistributedCacheEntryOptions
                                                {
                                                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expireTime)
                                                });
                }

                return product;
            }

            return JsonConvert.DeserializeObject<T>(productCache);
        }

        public async Task AddAsync(T entity, int expireTime = 60)
        {
            var options = new InsertOneOptions { BypassDocumentValidation = false };

            await Collection
                    .InsertOneAsync(entity, options);

            await _redisCache.SetStringAsync(string.Format(REDIS_CACHE_BY_ID, typeof(T).Name, entity.Id)
                                                , JsonConvert.SerializeObject(entity)
                                                , new DistributedCacheEntryOptions
                                                {
                                                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expireTime)
                                                });
        }
        public async Task<bool> UpdateAsync(T entity, int expireTime = 60)
        {
            var updateResult = await Collection
                                .ReplaceOneAsync(filter: g => g.Id == entity.Id, replacement: entity);

            if (updateResult.IsAcknowledged && updateResult.ModifiedCount > 0)
            {
                await _redisCache.SetStringAsync(string.Format(REDIS_CACHE_BY_ID, typeof(T).Name, entity.Id)
                                                , JsonConvert.SerializeObject(entity)
                                                , new DistributedCacheEntryOptions
                                                {
                                                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expireTime)
                                                });

                return true;
            }

            return false;
        }
        public async Task<bool> DeleteAsync(string id)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await Collection
                                                .DeleteOneAsync(filter);

            if (deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0)
            {
                await _redisCache.RemoveAsync(string.Format(REDIS_CACHE_BY_ID, typeof(T).Name, id));

                return true;
            }

            return false;
        }
    }
}
