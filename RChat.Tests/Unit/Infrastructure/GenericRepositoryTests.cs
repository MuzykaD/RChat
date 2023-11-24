
using Microsoft.EntityFrameworkCore;
using RChat.Domain.Chats;
using RChat.Domain.Common;
using RChat.Domain.Users;
using RChat.Infrastructure.Common;
using RChat.Infrastructure.Context;
using RChat.Infrastructure.Contracts.Common;
using Xunit;

namespace RChat.Tests.Unit.Infrastructure
{
    //In-memory Db
    public class GenericRepositoryTests : IDisposable
    {
        private static DbContextOptions<RChatDbContext> DbContextOptions =>
            new DbContextOptionsBuilder<RChatDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
        private RChatDbContext _testDbContext;
        private IRepository<TEntity, int> GetRepository<TEntity>() where TEntity : class, IDbEntity<int>
        {
            return new Repository<TEntity, int>(_testDbContext);
        }

        public GenericRepositoryTests()
        {
            _testDbContext = new RChatDbContext(DbContextOptions);
            _testDbContext.Database.EnsureCreated();
            SeedDatabase(_testDbContext);
        }

        [Fact]
        public async Task CreateAsync_Should_Add_Entity_To_Database()
        {
            // Arrange
            var repository = GetRepository<Chat>();
            string chatName = "NewTestChat";
            var newChat = new Chat { Name = chatName, IsGroupChat = true };

            // Act
            await repository.CreateAsync(newChat);
            await _testDbContext.SaveChangesAsync();

            // Assert
            var addedChat = await _testDbContext.Chats.FirstOrDefaultAsync(c => c.Name == chatName);
            Assert.NotNull(addedChat);
            Assert.Equal(chatName, addedChat.Name);
        }
        /*  ExecuteDeleteAsync could not be translated into In-Memory linq query
        [Fact]
        public async Task DeleteAsync_Should_Remove_Entity_From_Database()
        {
            // Arrange
            var repository = GetRepository<Chat>();
            var chatToDelete = await _testDbContext.Chats.FirstOrDefaultAsync();
            // Act
            await repository.DeleteAsync(chatToDelete!.Id);

            // Assert
            var deletedChat = await _testDbContext.Chats.FindAsync(chatToDelete.Id);
            Assert.Null(deletedChat);
        }
        */

        [Fact]
        public async Task GetAllAsync_Should_Return_All_User_Entities()
        {
            // Arrange
            var repository = GetRepository<User>();

            // Act
            var allUsers = await repository.GetAllAsync();

            // Assert
            Assert.Equal(3, allUsers.Count());
        }

        [Fact]
        public async Task GetAllAsync_With_Condition_Should_Return_Entities_Satisfying_Condition()
        {
            // Arrange
            var repository = GetRepository<User>();
            // Act
            var filteredUsers = await repository.GetAllAsync(u => u.UserName.Contains("d"));
            // Assert
            Assert.Equal(2, filteredUsers.Count());
        }
        [Fact]
        public async Task GetAllIncluding_Should_Return_Entities_With_Included_Properties()
        {
            // Arrange
            var repository = GetRepository<Chat>();

            // Act
            var chatsIncludingUsers = await repository.GetAllIncluding(c => c.Users).ToListAsync();

            // Assert
            Assert.All(chatsIncludingUsers, chat =>
            {
                Assert.NotNull(chat.Users);
                Assert.NotEmpty(chat.Users);
            });
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Entity_With_Given_Id()
        {
            // Arrange
            var repository = GetRepository<User>();
            var userId = (await _testDbContext.Users.FirstAsync()).Id;
            // Act
            var user = await repository.GetByIdAsync(userId);
            // Assert
            Assert.NotNull(user);
            Assert.Equal(userId, user.Id);
        }

        [Fact]
        public async Task GetByConditionAsync_Should_Return_Entity_Satisfying_Condition()
        {
            // Arrange
            var repository = GetRepository<User>();

            // Act
            var user = await repository.GetByConditionAsync(u => u.UserName == "First");

            // Assert
            Assert.NotNull(user);
            Assert.Equal("First", user.UserName);
        }

        [Fact]
        public async Task Update_Should_Update_Entity_In_Database()
        {
            // Arrange
            string oldChatName = "ChatToBeUpdated";
            var repository = GetRepository<Chat>();
            var userToUpdate = await _testDbContext.Chats.FirstOrDefaultAsync(u => u.Name == oldChatName);
            var newChatName = "UpdateChatName";
            var isGroupChat = true;
            userToUpdate.Name = newChatName;
            userToUpdate.IsGroupChat = isGroupChat;

            // Act
            repository.Update(userToUpdate);
            await _testDbContext.SaveChangesAsync();
            // Assert
            var updatedChat = await _testDbContext.Chats.FindAsync(userToUpdate.Id);
            Assert.NotNull(updatedChat);
            Assert.Equal(newChatName, updatedChat.Name);
            Assert.NotEqual(oldChatName, updatedChat.Name);
        }


        public void Dispose()
        {
            _testDbContext.Database.EnsureDeleted();
        }

        private static void SeedDatabase(RChatDbContext context)
        {
            var user1 = new User() { UserName = "First", Email = "first@mail.com" };
            var user2 = new User() { UserName = "Second", Email = "second@mail.com" };
            var user3 = new User() { UserName = "Third", Email = "third@mail.com" };
            var chats = new List<Chat>
            {
                new() {Name = "TestChat1", IsGroupChat = false, Users =  new List<User> { user1, user2 } },
                new() {Name = "TestChat@", IsGroupChat = true, Users =  new List<User> { user1, user2, user3 } },
                new() {Name = "ChatToBeUpdated", IsGroupChat = false, Users =  new List<User> { user2, user3 } },
            };
             context.Chats.AddRange(chats);
             context.SaveChanges();

        }
    }
}
