using Microsoft.EntityFrameworkCore;
using RChat.Domain.Common;
using RChat.Infrastructure.Context;
using RChat.Infrastructure.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Infrastructure.Common
{
    internal class Repository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : class ,IDbEntity<TId>
    {
        private RChatDbContext _context;
        public Repository(RChatDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public async Task DeleteAsync(TId id)
        {
           await _context.Set<TEntity>()
                .Where(entity => entity.Id.Equals(id))
                .ExecuteDeleteAsync();
        }

        public async Task<IQueryable<TEntity>> GetAllAsQueryableAsync()
        {
            var query = _context.Set<TEntity>().AsQueryable();

            return await Task.FromResult(query);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(TId id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }
    }
}
