using RChat.Domain.Users;
using RChat.Infrastructure.Context;
using RChat.Infrastructure.Contracts.Users;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace RChat.Infrastructure.Users
{
    public class UserRepository : IUserRepository
    {
        private RChatDbContext _context;
        public UserRepository(RChatDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _context.Users.Where(u => u.Id.Equals(id)).ExecuteDeleteAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task UpdateAsync(User entity)
        {
            await _context.Users.Where(u => u.Id.Equals(entity.Id))
                                .ExecuteUpdateAsync(
                                    u => 
                                    u.SetProperty(
                                    selectedProperty => selectedProperty.FirstName,
                                    propertyValue => entity.FirstName));
        }
    }
}
