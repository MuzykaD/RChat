using RChat.Domain.Common;
using RChat.Infrastructure.Contracts.Common;

namespace RChat.Infrastructure.Contracts.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<TEntity, TId> GetRepository<TEntity, TId>() where TEntity : class, IDbEntity<TId>;
        Task SaveChangesAsync();
    }
}
