using Hb.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Hb.Domain.Repositories.Base
{
    public interface IRepository<T> where T : class, IEntityBase, new()
    {
        IQueryable<T> Get(Expression<Func<T, bool>> predicate = null);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetByIdAsync(string id, int expireTime = 60);
        Task AddAsync(T entity, int expireTime = 60);
        Task<bool> UpdateAsync(T entity, int expireTime = 60);
        Task<bool> DeleteAsync(string id);
    }
}
