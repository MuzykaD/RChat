using Moq;
using RChat.Application.Authentication;
using RChat.Application.Contracts.Authentication;
using RChat.Application.Contracts.Authentication.JWT;
using RChat.Domain.Users;
using RChat.Domain.Users.DTO;
using RChat.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RChat.Tests.Unit.Application.Authentication
{
    public class AuthenticationServiceTests
    {
        List<User> TestUserList => new List<User>()
        {
            new(){Id = 1, UserName = "User1", Email="user1@mail.com",PhoneNumber = "111111111"},
            new(){Id = 2, UserName = "User2", Email="user2@mail.com",PhoneNumber = "222222222"},
            new(){Id = 3, UserName = "User3", Email="user3@mail.com",PhoneNumber = "333333333"},
        };

        [Fact]
        public async Task RegisterUserAsync_ValidData_UserCreated()
        {
            //Arrange
            var userList = TestUserList;
            var managerMock = MockHelper.GetMockUserManager(userList);
            var jwtServiceMock = new Mock<IJwtTokenService>();
            IAuthenticationService sut = new AuthenticationService(jwtServiceMock.Object, managerMock.Object);
            var createdUser = new RegisterUserDto()
            {
                Email = "newuser@mail.com",
                Username = "newUser",
                Password = "Nnnn1!"
            };
            //Act
            var isRegistered = await sut.RegisterUserAsync(createdUser);
            //Assert
            Assert.True(isRegistered);
            Assert.True(userList.Exists(u => u.Email == createdUser.Email));
        }

        [Fact]
        public async Task RegisterUserAsync_ExistingEmail_UserNotCreated()
        {
            //Arrange
            var userList = TestUserList;
            var managerMock = MockHelper.GetMockUserManager(userList);
            managerMock.Setup(m => m.FindByEmailAsync("user1@mail.com"))
                .ReturnsAsync(TestUserList.FirstOrDefault(u => u.Email == "user1@mail.com"));
            var jwtServiceMock = new Mock<IJwtTokenService>();
            IAuthenticationService sut = new AuthenticationService(jwtServiceMock.Object, managerMock.Object);

            var createdUser = new RegisterUserDto()
            {
                Email = "user1@mail.com",
                Username = "newUser",
                Password = "Nnnn1!"
            };
            //Act
            var isRegistered = await sut.RegisterUserAsync(createdUser);
            //Assert
            Assert.False(isRegistered);
            managerMock.Verify(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
        }
        [Fact]
        public async Task GetTokenByCredentials_ValidData_InvokesMethod_ReturnsString()
        {
            //Arrange
            var managerMock = MockHelper.GetMockUserManager(TestUserList);
            managerMock
                .Setup(m => m.FindByEmailAsync("user1@mail.com"))
                .ReturnsAsync(TestUserList.FirstOrDefault(u => u.Email == "user1@mail.com"));
            managerMock
                .Setup(m => m.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).
                ReturnsAsync(true);
            var jwtServiceMock = new Mock<IJwtTokenService>();
            jwtServiceMock
                .Setup(j => j.GenerateJwtTokenAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync("ValidToken");
            IAuthenticationService sut = new AuthenticationService(jwtServiceMock.Object, managerMock.Object);
            //Act
            var token = await sut.GetTokenByCredentials("user1@mail.com", "dummyPassword");
            //Assert
            Assert.Equal("ValidToken", token);
            jwtServiceMock.Verify(j => j.GenerateJwtTokenAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
        [Fact]
        public async Task GetTokenByCredentials_NonExistingEmail_ReturnsNull()
        {
            //Arrange
            var managerMock = MockHelper.GetMockUserManager(TestUserList);
            var jwtServiceMock = new Mock<IJwtTokenService>();
            IAuthenticationService sut = new AuthenticationService(jwtServiceMock.Object, managerMock.Object);
            //Act
            var token = await sut.GetTokenByCredentials("dummy@mail.com", "dummyPassword");
            //Assert
            Assert.Null(token);
            jwtServiceMock.Verify(j => j.GenerateJwtTokenAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);

        }
        [Fact]
        public async Task GetTokenByCredentials_NotValidPassword_ReturnsNull()
        {
            //Arrange
            var managerMock = MockHelper.GetMockUserManager(TestUserList);
            managerMock
                .Setup(m => m.FindByEmailAsync("user1@mail.com"))
                .ReturnsAsync(TestUserList.FirstOrDefault(u => u.Email == "user1@mail.com"));
            managerMock
                .Setup(m => m.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).
                ReturnsAsync(false);
            var jwtServiceMock = new Mock<IJwtTokenService>();          
            IAuthenticationService sut = new AuthenticationService(jwtServiceMock.Object, managerMock.Object);
            //Act
            var token = await sut.GetTokenByCredentials("user1@mail.com", "dummyPassword");
            //Assert
            Assert.Null(token);
            jwtServiceMock.Verify(j => j.GenerateJwtTokenAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);

        }
    }
}
