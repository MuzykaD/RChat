using RChat.Domain.Users;
using RChat.Infrastructure.Contracts.Common;

namespace RChat.Infrastructure.Contracts.Users
{
    public interface IUserRepository : IRepository<User>
    {
    }
}
