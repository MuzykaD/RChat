using Moq;
using RChat.Domain.Users;
using RChat.Domain;
using RChat.Infrastructure.Contracts.Common;
using RChat.Infrastructure.Contracts.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using RChat.Domain.Chats;
using RChat.Application.Chats;
using System.Linq.Expressions;

namespace RChat.Tests.Unit.Application.Chats
{
    public class ChatServiceTests
    {
        [Fact]
        public async Task CreatePublicGroupAsync_CreatesGroup_ReturnsTrue()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userRepoMock = new Mock<IRepository<User, int>>();
            var chatRepoMock = new Mock<IRepository<Chat, int>>();

            unitOfWorkMock.Setup(u => u.GetRepository<User, int>()).Returns(userRepoMock.Object);
            unitOfWorkMock.Setup(u => u.GetRepository<Chat, int>()).Returns(chatRepoMock.Object);

            var chatService = new ChatService(unitOfWorkMock.Object);

            var groupName = "TestGroup";
            var creatorId = 1;
            var membersId = new List<int> { 2, 3 };

            // Act
            var result = await chatService.CreatePublicGroupAsync(groupName, creatorId, membersId);

            // Assert
            chatRepoMock.Verify(repo => repo.CreateAsync(It.IsAny<Chat>()), Times.Once);
            unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetChatsInformationListAsync_ReturnsCorrectGridListDto()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var chatRepoMock = new Mock<IRepository<Chat, int>>();

            unitOfWorkMock.Setup(u => u.GetRepository<Chat, int>()).Returns(chatRepoMock.Object);

            var sut = new ChatService(unitOfWorkMock.Object);

            var searchArguments = new SearchArguments(null, 1, 2, null, null);


            var chats = new List<Chat>
        {
            new Chat { Id = 1, Name = "Group1", Creator = new User { UserName = "User1" }, Users = new List<User> { new User(), new User() }, IsGroupChat = true },
            new Chat { Id = 2, Name = "Group2", Creator = new User { UserName = "User2" }, Users = new List<User> { new User() }, IsGroupChat = true },
            new Chat { Id = 3, Name = "Group3", Users = new List<User> { new User() }, IsGroupChat = false },
        }.AsQueryable();

            var mockSet = new Mock<DbSet<Chat>>();
            mockSet.As<IQueryable<Chat>>().Setup(m => m.Provider).Returns(chats.Provider);
            mockSet.As<IQueryable<Chat>>().Setup(m => m.Expression).Returns(chats.Expression);
            mockSet.As<IQueryable<Chat>>().Setup(m => m.ElementType).Returns(chats.ElementType);
            mockSet.As<IQueryable<Chat>>().Setup(m => m.GetEnumerator()).Returns(chats.GetEnumerator());

            chatRepoMock.Setup(c => c.GetAllAsQueryable()).Returns(mockSet.Object);

            // Act
            var result = await sut.GetChatsInformationListAsync(searchArguments);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.TotalCount);
            Assert.Equal(2, result.SelectedEntities.Count());
        }

        [Fact]
        public async Task GetGroupChatByIdAsync_GroupExists_ReturnsChat()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var chatRepoMock = new Mock<IRepository<Chat, int>>();

            unitOfWorkMock.Setup(u => u.GetRepository<Chat, int>()).Returns(chatRepoMock.Object);

            var chatService = new ChatService(unitOfWorkMock.Object);

            var currentUserId = 1;
            var chatId = 1;

            var user = new User { Id = currentUserId };
            var chats = new List<Chat>() {
                new Chat { Id = chatId, Users = new List<User> { user }, IsGroupChat = true } ,
                new Chat { Id = 2, Users = new List<User> { user }, IsGroupChat = false } ,
            }.AsQueryable();



            chatRepoMock.Setup(c => c
            .GetAllIncluding(It.IsAny<Expression<Func<Chat, object>>>(),
                             It.IsAny<Expression<Func<Chat, object>>>()))
                .Returns(chats);

            // Act
            var result = await chatService.GetGroupChatByIdAsync(currentUserId, chatId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsGroupChat);
            Assert.Contains<Chat>(result, chats);
        }

        [Fact]
        public async Task GetPrivateChatByUsersIdAsync_ChatExists_ShouldReturnExistingChat()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var chatRepoMock = new Mock<IRepository<Chat, int>>();
            var userRepoMock = new Mock<IRepository<User, int>>();

            unitOfWorkMock.Setup(u => u.GetRepository<Chat, int>()).Returns(chatRepoMock.Object);
            unitOfWorkMock.Setup(u => u.GetRepository<User, int>()).Returns(userRepoMock.Object);

            var chatService = new ChatService(unitOfWorkMock.Object);

            var currentUserId = 1;
            var secondUserId = 2;

            var existingChat = new List<Chat>(){ new Chat
            {
                IsGroupChat = false,
                Users = new List<User> { new User { Id = currentUserId }, new User { Id = secondUserId } },
            }
            }.AsQueryable();
            chatRepoMock.Setup(c => c.GetAllIncluding(It.IsAny<Expression<Func<Chat, object>>>(), It.IsAny<Expression<Func<Chat, object>>>())).Returns(existingChat);
            // Act
            var result = await chatService.GetPrivateChatByUsersIdAsync(currentUserId, secondUserId);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingChat.First(), result);
            chatRepoMock.Verify(c => c.CreateAsync(It.IsAny<Chat>()), Times.Never);
        }

        [Fact]
        public async Task GetPrivateChatByUsersIdAsync_NonExistingChat_ReturnsNewChat()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var chatRepoMock = new Mock<IRepository<Chat, int>>();
            var userRepoMock = new Mock<IRepository<User, int>>();

            unitOfWorkMock.Setup(u => u.GetRepository<Chat, int>()).Returns(chatRepoMock.Object);
            unitOfWorkMock.Setup(u => u.GetRepository<User, int>()).Returns(userRepoMock.Object);

            var chatService = new ChatService(unitOfWorkMock.Object);

            var currentUserId = 1;
            var secondUserId = 2;

            var existingChat = new List<Chat>(){ new Chat
            {
                IsGroupChat = false,
                Users = new List<User> { new User { Id = 3 }, new User { Id = secondUserId } },
            }
            }.AsQueryable();
            chatRepoMock.Setup(c => c.GetAllIncluding(It.IsAny<Expression<Func<Chat, object>>>(), It.IsAny<Expression<Func<Chat, object>>>())).Returns(existingChat);
            // Act
            var result = await chatService.GetPrivateChatByUsersIdAsync(currentUserId, secondUserId);
            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(existingChat.First(), result);
            chatRepoMock.Verify(c => c.CreateAsync(It.IsAny<Chat>()), Times.Once);
        }

        [Fact]
        public async Task GetGroupsIdentifiersAsync_ReturnValidId()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var chatRepoMock = new Mock<IRepository<Chat, int>>();

            unitOfWorkMock.Setup(u => u.GetRepository<Chat, int>()).Returns(chatRepoMock.Object);
            var chatService = new ChatService(unitOfWorkMock.Object);
            var user = new User() { Id = 1 };
            var anotherUser = new User() { Id = 2 };
            var existingChats = new List<Chat>(){
            new Chat
            {
                Id = 1,
                Users = new List<User> { user },

            }, new Chat
            {
                Id = 2,
                Users = new List<User> { user, anotherUser },

            }, new Chat
            {
                Id = 3,
                Users = new List<User> { anotherUser },
            },
            }.AsQueryable();
            chatRepoMock
                .Setup(c => c.GetAllIncluding(It.IsAny<Expression<Func<Chat, object>>>()))
                .Returns(existingChats);
            // Act
            var result = await chatService.GetGroupsIdentifiersAsync(user.Id);
            // Assert
            Assert.NotEmpty(result);
            Assert.Equivalent(existingChats.Where(c => c.Users.Contains(user)).Select(c => c.Id), result);
        }
    }
}
