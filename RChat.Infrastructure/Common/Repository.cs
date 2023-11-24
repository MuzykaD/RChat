using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RChat.Domain.Chats;
using RChat.Domain.Common;
using RChat.Infrastructure.Context;
using RChat.Infrastructure.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Infrastructure.Common
{
    public class Repository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : class, IDbEntity<TId>
    {
        private RChatDbContext _context;
        private DbSet<TEntity> Table => _context.Set<TEntity>();
        public Repository(RChatDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(TEntity entity)
        {
            await Table.AddAsync(entity);
        }

        public async Task DeleteAsync(TId id)
        {
            await Table
                 .Where(entity => entity.Id.Equals(id))
                 .ExecuteDeleteAsync();
        }

        public IQueryable<TEntity> GetAllAsQueryable()
        {

            return Table.AsQueryable();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Table.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> condition)
        {
            return await Table.Where(condition).ToListAsync();
        }

        public IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var query = Table.AsQueryable();

            if (!propertySelectors.IsNullOrEmpty())
            {
                foreach (var propertySelector in propertySelectors)
                {
                    query = query.Include(propertySelector);
                }
            }

            return query;
        }

        public async Task<TEntity?> GetByConditionAsync(Expression<Func<TEntity, bool>> condition)
        {
            return await Table.FirstOrDefaultAsync(condition);
        }

        public async Task<TEntity?> GetByIdAsync(TId id)
        {
            return await Table.FindAsync(id);
        }

        public void Update(TEntity entity)
        {
            Table.Update(entity);
        }
    }
}
