using Microsoft.EntityFrameworkCore;
using RChat.Domain.Messages;
using RChat.Infrastructure.Context;
using RChat.Infrastructure.Contracts.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Infrastructure.Messages
{
    public class MessageRepository : IMessageRepository
    {
        private RChatDbContext _context;
        public MessageRepository(RChatDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(Message entity)
        {
            await _context.Messages.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _context.Messages.Where(m => m.Id.Equals(id)).ExecuteDeleteAsync();
        }

        public async Task<IQueryable<Message>> GetAllAsQueryableAsync()
        {
            var query = _context.Messages.AsQueryable();
            return await Task.FromResult(query);
        }

        public async Task<IEnumerable<Message>> GetAllAsync()
        {
            return await _context.Messages.ToListAsync();
        }

        public async Task<Message?> GetByIdAsync(int id)
        {
            return await _context.Messages.FindAsync(id);
        }
        //TODO 
        public async Task UpdateAsync(Message entity)
        {
           await _context.Messages.ExecuteUpdateAsync(m =>
                                    m.SetProperty(
                                    selectedProperty => selectedProperty.Content,
                                    propertyValue => entity.Content));
        }
    }
}
