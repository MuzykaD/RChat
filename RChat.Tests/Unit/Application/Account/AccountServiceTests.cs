using RChat.Application.Account;
using RChat.Application.Contracts.Account;
using RChat.Domain.Users;
using RChat.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using RChat.Domain.Users.DTO;
using Microsoft.AspNet.Identity;
using Moq;

namespace RChat.Tests.Unit.Application.Account
{
    public class AccountServiceTests
    {
        List<User> TestUserList => new List<User>()
        {
            new(){Id = 1, UserName = "User1", Email="user1@mail.com",PhoneNumber = "111111111"},
            new(){Id = 2, UserName = "User2", Email="user2@mail.com",PhoneNumber = "222222222"},
            new(){Id = 3, UserName = "User3", Email="user3@mail.com",PhoneNumber = "333333333"},
        };
        

        [Theory]
        [InlineData("user1@mail.com")]
        [InlineData("user2@mail.com")]
        [InlineData("user3@mail.com")]
        public async Task UpdateUser_ValidEmail_ReturnsTrue(string currentEmail)
        {
            //Arrange
            var mockManager = MockHelper.GetMockUserManager(TestUserList);
            mockManager.Setup(m => m.FindByEmailAsync(currentEmail))
                .ReturnsAsync(TestUserList.FirstOrDefault(u => u.Email == currentEmail));
            IAccountService service = new AccountService(mockManager.Object);
            var updatedUser = new UpdateUserDto()
            {
                UserName = "updatedUserName",
                Email = "updatedUserEmail@mail.com",
                PhoneNumber = "1234567890",
            };
            //Act
            var isUpdated = await service.UpdateUserAsync(currentEmail, updatedUser);
            //Assert
            Assert.True(isUpdated);
        }
        [Theory]
        [InlineData("user11@mail.com")]
        [InlineData("user22@mail.com")]
        [InlineData("user33@mail.com")]
        public async Task UpdateUser_NonExistingEmail_ReturnsFalse(string currentEmail)
        {
            //Arrange
            var mockManager = MockHelper.GetMockUserManager(TestUserList);
            mockManager.Setup(m => m.FindByEmailAsync(currentEmail));
            IAccountService service = new AccountService(mockManager.Object);
            var updatedUser = new UpdateUserDto()
            {
                UserName = "updatedUserName",
                Email = "updatedUserEmail@mail.com",
                PhoneNumber = "1234567890",
            };
            //Act
            var isUpdated = await service.UpdateUserAsync(currentEmail, updatedUser);
            //Assert
            Assert.False(isUpdated);
        }
        [Theory]
        [InlineData("user1@mail.com")]
        [InlineData("user2@mail.com")]
        [InlineData("user3@mail.com")]
        public async Task GetUserInformation_ValidEmail_ReturnsValidInformationDto(string currentEmail)
        {
            //Arrange
            var mockManager = MockHelper.GetMockUserManager(TestUserList);
            mockManager.Setup(m => m.FindByEmailAsync(currentEmail))
                .ReturnsAsync(TestUserList.FirstOrDefault(u => u.Email == currentEmail));
            IAccountService service = new AccountService(mockManager.Object);
            var user = TestUserList.FirstOrDefault(u => u.Email == currentEmail);
            //Act
            var userInformationDto = await service.GetPersonalInformationAsync(currentEmail);
            //Assert
            Assert.NotNull(userInformationDto);
            Assert.Equal(user.Id, userInformationDto.Id);
            Assert.Equal(user.UserName, userInformationDto.UserName);
            Assert.Equal(user.Email, userInformationDto.Email);
            Assert.Equal(user.PhoneNumber, userInformationDto.PhoneNumber);
        }

        [Theory]
        [InlineData("user11@mail.com")]
        [InlineData("user22@mail.com")]
        [InlineData("user33@mail.com")]
        public async Task GetUserInformation_NonExistingEmail_ReturnsNull(string currentEmail)
        {
            //Arrange
            var mockManager = MockHelper.GetMockUserManager(TestUserList);
            mockManager.Setup(m => m.FindByEmailAsync(currentEmail))
                .ReturnsAsync(TestUserList.FirstOrDefault(u => u.Email == currentEmail));
            IAccountService service = new AccountService(mockManager.Object);
            var user = TestUserList.FirstOrDefault(u => u.Email == currentEmail);
            //Act
            var userInformationDto = await service.GetPersonalInformationAsync(currentEmail);
            //Assert
            Assert.Null(userInformationDto);
           
        }
    }
}
