using FluentAssertions;
using Moq;
using RChat.Application.Contracts.Messages;
using RChat.Application.Messages;
using RChat.Domain;
using RChat.Domain.Messages;
using RChat.Domain.Messages.Dto;
using RChat.Infrastructure.Contracts.Common;
using RChat.Infrastructure.Contracts.UnitOfWork;
using System.Data.Entity;
using Xunit;

namespace RChat.Tests.Unit.Application.Messages
{
    public class MessageServiceTests
    {
        [Fact]
        public async Task CreateMessageAsync_CreatesMessage()
        {
            var messageList = new List<Message>();
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var messageRepoMock = new Mock<IRepository<Message, int>>();
            messageRepoMock
                .Setup(mr => mr.CreateAsync(It.IsAny<Message>()))
                .Callback<Message>(m => messageList.Add(m));
            unitOfWorkMock
                .Setup(u => u.GetRepository<Message, int>())
                .Returns(messageRepoMock.Object);

            var messageService = new MessageService(unitOfWorkMock.Object);

            var message = new MessageInformationDto()
            {
                ChatId = 1,
                Content = "dummy content",
                SentAt = DateTime.Now,
            };
            int senderId = 1;

            // Act
            await messageService.CreateMessageAsync(senderId, message);

            // Assert
            messageRepoMock.Verify(repo => repo.CreateAsync(It.IsAny<Message>()), Times.Once);
            unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
            Assert.NotEmpty(messageList);
        }
        [Fact]
        public async Task DeleteMessageAsync_ValidDate_ReturnsTrue()
        {
            var messageList = new List<Message>() { new Message() { Id = 1, SenderId = 1} };
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var messageRepoMock = new Mock<IRepository<Message, int>>();
            messageRepoMock
                .Setup(m => m.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(messageList.FirstOrDefault(m => m.Id == 1));
            messageRepoMock
                .Setup(mr => mr.DeleteAsync(It.IsAny<int>()))
                .Callback<int>(id => messageList = messageList.Where(m => m.Id != id).ToList());
            unitOfWorkMock
                .Setup(u => u.GetRepository<Message, int>())
                .Returns(messageRepoMock.Object);

            var messageService = new MessageService(unitOfWorkMock.Object);

            var message = new MessageInformationDto()
            {
                ChatId = 1,
                Content = "dummy content",
                SentAt = DateTime.Now,
            };
            int senderId = 1;
            int messageId = 1;

            // Act
            await messageService.DeleteMessageAsync(messageId, senderId);

            // Assert
            messageRepoMock.Verify(repo => repo.DeleteAsync(It.IsAny<int>()), Times.Once);
            unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Never);
            Assert.Empty(messageList);
        }
        [Fact]
        public async Task GetMessagesInformationListAsync_ReturnsCorrectGridListDto()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var messageRepoMock = new Mock<IRepository<Message, int>>();

            unitOfWorkMock.Setup(u => u.GetRepository<Message, int>()).Returns(messageRepoMock.Object);

            IMessageService sut = new MessageService(unitOfWorkMock.Object);

            var searchArguments = new SearchArguments(null, 0, 3, "Content", "Ascending");

            var messages = new List<Message>()
            {
                new(){Content = "c", ChatId = 1, SenderId = 1},
                new(){Content = "a", ChatId = 1, SenderId = 1},
                new(){Content = "b", ChatId = 2, SenderId = 1},
                new(){Content = "d", ChatId = 3, SenderId = 1},
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Message>>();
            mockSet.As<IQueryable<Message>>().Setup(m => m.Provider).Returns(messages.Provider);
            mockSet.As<IQueryable<Message>>().Setup(m => m.Expression).Returns(messages.Expression);
            mockSet.As<IQueryable<Message>>().Setup(m => m.ElementType).Returns(messages.ElementType);
            mockSet.As<IQueryable<Message>>().Setup(m => m.GetEnumerator()).Returns(messages.GetEnumerator());

            messageRepoMock.Setup(c => c.GetAllAsQueryable()).Returns(mockSet.Object);

            // Act
            var result = await sut.GetMessagesInformationListAsync(searchArguments);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.TotalCount);
            Assert.Equal(3, result.SelectedEntities.Count());
            result.SelectedEntities.Should().BeInAscendingOrder(p => p.Content);
        }

        [Fact]
        public async Task UpdateMessageAsync_ValidData_MessageUpdated()
        { // Arrange
            int senderId = 1;
            int messageId = 1;
            string oldContent = "old content";
            var message = new MessageInformationDto()
            {
                Id = messageId,
                ChatId = 1,
                Content = "dummy content",
                SentAt = DateTime.Now,
                SenderId = senderId,
            };
            var messageList = new List<Message>() { new Message() 
            { Id = messageId, SenderId = senderId, Content = oldContent } };           
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var messageRepoMock = new Mock<IRepository<Message, int>>();
            messageRepoMock
                .Setup(m => m.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(messageList.FirstOrDefault(m => m.Id == messageId));
            messageRepoMock
                .Setup(mr => mr.Update(It.IsAny<Message>()))
                .Callback<Message>(id => messageList.FirstOrDefault(m => m.Id == messageId).Content = message.Content);
            unitOfWorkMock
                .Setup(u => u.GetRepository<Message, int>())
                .Returns(messageRepoMock.Object);
            var messageService = new MessageService(unitOfWorkMock.Object);           
            // Act
           var result = await messageService.UpdateMessageAsync(senderId, message);
            // Assert
            messageRepoMock.Verify(repo => repo.Update(It.IsAny<Message>()), Times.Once);
            unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
            Assert.Equal(message.Content, messageList.FirstOrDefault(m => m.Id == messageId).Content);
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateMessageAsync_InvalidData_MessageNotUpdated()
        { // Arrange
            int randomUserId = 23;
            int senderId = 999;
            int messageId = 1;
            string oldContent = "old content";
            var message = new MessageInformationDto()
            {
                Id = messageId,
                ChatId = 1,
                Content = "dummy content",
                SentAt = DateTime.Now,
                SenderId = randomUserId,
            };
            var messageList = new List<Message>() { new Message()
            { Id = messageId, SenderId = senderId, Content = oldContent } };
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var messageRepoMock = new Mock<IRepository<Message, int>>();
            messageRepoMock
                .Setup(m => m.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(messageList.FirstOrDefault(m => m.Id == messageId));
            unitOfWorkMock
                .Setup(u => u.GetRepository<Message, int>())
                .Returns(messageRepoMock.Object);
            var messageService = new MessageService(unitOfWorkMock.Object);
            // Act
            var result = await messageService.UpdateMessageAsync(randomUserId, message);
            // Assert
            messageRepoMock.Verify(repo => repo.Update(It.IsAny<Message>()), Times.Never);
            unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Never);
            Assert.NotEqual(message.Content, messageList.FirstOrDefault(m => m.Id == messageId).Content);
            Assert.False(result);
        }

    }
}
