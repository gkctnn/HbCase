using Hb.Catalog.Infrastructures.Data.Interfaces;
using Hb.Catalog.Infrastructures.Repositories.Base;
using Hb.Catalog.Infrastructures.Settings;
using Hb.Domain.Entities;
using Hb.Domain.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;

namespace Hb.Catalog.Infrastructures.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(IOptions<DatabaseSettings> options
            , IDistributedCache redisCache)
            : base(options
                  , redisCache)
        {

        }
    }
}
