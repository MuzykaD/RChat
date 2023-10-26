using RChat.Infrastructure.Contracts.Common;

namespace RChat.Infrastructure.Contracts.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task<IRepository<T>> GetRepositoryAsync<T>();
        Task SaveChangesAsync();
    }
}
