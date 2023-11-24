using FluentAssertions;
using MockQueryable.Moq;
using RChat.Application.Contracts.Users;
using RChat.Application.Users;
using RChat.Domain;
using RChat.Domain.Users;
using RChat.Tests.Helpers;
using Xunit;

namespace RChat.Tests.Unit.Application.Users
{
    public class UserServiceTests
    {
        List<User> TestUserList => new List<User>()
        {
            new(){Id = 1, UserName = "User1", Email="user1@mail.com",PhoneNumber = "111111111"},
            new(){Id = 2, UserName = "User2", Email="user2@mail.com",PhoneNumber = "222222222"},
            new(){Id = 3, UserName = "User3", Email="user3@mail.com",PhoneNumber = "333333333"},
        };
        [Fact]
        public async Task GetUsersInformationListAsync_Returns_ReturnsCorrectGridListDto()
        {
            //Arrange
            var userList = TestUserList;
            var userManagerMock = MockHelper.GetMockUserManager(userList);
            userManagerMock.Setup(c => c.Users).Returns(userList.BuildMock());
            var searchArguments = new SearchArguments(null, 1, 2, "UserName", "Ascending");
            IUserService sut = new UserService(userManagerMock.Object);
            //Act
            var result = await sut.GetUsersInformationListAsync(searchArguments);
            //Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.TotalCount);
            Assert.Equal(2, result.SelectedEntities.Count());
            result.SelectedEntities.Should().BeInAscendingOrder(p => p.UserName);
        }
    }
}
