using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Infrastructure.Contracts.Common
{
    public interface IRepository<TEntity, TId>
    {
        Task<TEntity?> GetByIdAsync(TId id);
        Task<TEntity?> GetByConditionAsync(Expression<Func<TEntity, bool>> condition);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> condition);
        IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors);
        IQueryable<TEntity> GetAllAsQueryable();
        Task CreateAsync(TEntity entity);
        void Update(TEntity entity);
        Task DeleteAsync(TId id);
    }
}
