using RChat.Domain.Chats;
using RChat.Domain.Common;
using RChat.Domain.Messages;
using RChat.Domain.Users;
using RChat.Infrastructure.Common;
using RChat.Infrastructure.Context;
using RChat.Infrastructure.Contracts.Common;
using RChat.Infrastructure.Contracts.UnitOfWork;
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

        public IRepository<TEntity, TId> GetRepository<TEntity, TId>() where TEntity : class, IDbEntity<TId> 
        {
            return new Repository<TEntity, TId>(_context);
        }
        

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
