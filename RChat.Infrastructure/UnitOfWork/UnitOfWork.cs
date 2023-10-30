using RChat.Domain.Users;
using RChat.Infrastructure.Context;
using RChat.Infrastructure.Contracts.Common;
using RChat.Infrastructure.Contracts.UnitOfWork;
using RChat.Infrastructure.Contracts.Users;
using RChat.Infrastructure.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private RChatDbContext _context;
        public UnitOfWork(RChatDbContext context)
        {
            _context = context;
        }


        public Task<IRepository<T>> GetRepositoryAsync<T>()
        {
            IRepository<T>? result = typeof(T).Name switch
            {
               nameof(User) => new UserRepository(_context) as IRepository<T>,
               _ => null
            };

            return Task.FromResult(result ?? throw new NotImplementedException($"Repository for type {typeof(T).Name} is not implemented"));
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
