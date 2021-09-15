using Hb.Catalog.Infrastructures.Repositories.Base;
using Hb.Catalog.Infrastructures.Settings;
using Hb.Domain.Entities;
using Hb.Domain.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace Hb.Catalog.Infrastructures.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(IOptions<DatabaseSettings> options
            , IDistributedCache redisCache)
            : base(options
                  , redisCache)
        {

        }
    }
}
