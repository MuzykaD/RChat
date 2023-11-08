using Microsoft.EntityFrameworkCore;
using RChat.Domain.Chats;
using RChat.Domain.Users;
using RChat.Infrastructure.Context;
using RChat.Infrastructure.Contracts.Chats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Infrastructure.Chats
{
    public class ChatRepository : IChatRepository
    {
        private RChatDbContext _context;
        public ChatRepository(RChatDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(Chat entity)
        {
            await _context.Chats.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _context.Chats.Where(c => c.Id.Equals(id)).ExecuteDeleteAsync();
        }

        public async Task<IEnumerable<Chat>> GetAllAsync()
        {
            return await _context.Chats.Include(c => c.Creator).Include(c => c.Messages).ToListAsync();
        }
        public async Task<IQueryable<Chat>> GetAllAsQueryableAsync()
        {
            var query = _context.Chats.Include(c => c.Creator).Include(c => c.Messages).AsQueryable();
            return await Task.FromResult(query);
        }

        public async Task<Chat?> GetByIdAsync(int id)
        {
            return await _context.Chats
                .Include(c => c.Creator)
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Id.Equals(id));
        }
        //TODO
        public async Task UpdateAsync(Chat entity)
        {
            await _context.Chats.Where(c => c.Id.Equals(entity.Id))
                                .ExecuteUpdateAsync(
                                    c =>
                                    c.SetProperty(
                                    selectedProperty => selectedProperty.Name,
                                    propertyValue => entity.Name));
        }
    }
}
