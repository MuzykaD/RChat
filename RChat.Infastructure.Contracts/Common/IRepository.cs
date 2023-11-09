using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Infrastructure.Contracts.Common
{
    public interface IRepository<TEntity, TId>
    {
        Task<TEntity?> GetByIdAsync(TId id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IQueryable<TEntity>> GetAllAsQueryableAsync();
        Task CreateAsync(TEntity entity);
        void Update(TEntity entity);
        Task DeleteAsync(TId id);
    }
}
