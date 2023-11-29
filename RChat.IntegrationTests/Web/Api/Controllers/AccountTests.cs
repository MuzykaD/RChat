using FluentAssertions;
using FluentAssertions.Execution;
using RChat.Domain.Users;
using RChat.Domain.Users.DTO;
using RChat.IntegrationTests.DbSqlHelpers.UserDbSqlHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace RChat.IntegrationTests.Web.Api.Controllers
{
    public class AccountTests : TestBase
    {
        private UserDbSqlHelper _userSqlHelper;
        private static RegisterUserDto TestAccountUser => new()
        {
            Username = "TestAccountUser",
            Email = "testAccountUser@mail.com",
            Password = "TestAccountUser1!"
        };
        public AccountTests()
        {
            _userSqlHelper = new UserDbSqlHelper();
        }
        [Fact]
        public async Task GetPersonalInformationAsync_ReturnsValidProfileData()
        {
            //Arrange
            var client =  factory.CreateClient();
            await client.PostAsJsonAsync("/api/v1/authentication/register", TestAccountUser);
            client = await factory.GetClientWithTokenAsync(new() { Email = TestAccountUser.Email, Password = TestAccountUser.Password });
            //Act
            var response = await client.GetAsync("/api/v1/account/profile");
            //Assert
            var result = await response.Content.ReadFromJsonAsync<UserInformationDto>();
            using(new AssertionScope())
            {
                response.EnsureSuccessStatusCode();
                result.Should().NotBeNull();
                result.UserName.Should().Be(TestAccountUser.Username);
                result.Email.Should().Be(TestAccountUser.Email);
            }

            //Clear data
            await _userSqlHelper.RemoveTestAuthenticationUsersAsync(TestAccountUser.Username);
        }

        [Fact]
        public async Task UpdateProfileAsync_ValidData_ReturnsTrue_ReturnsOkResult()
        {
            //Arrange
            var user = TestAccountUser;
            var client = factory.CreateClient();
            await client.PostAsJsonAsync("/api/v1/authentication/register", user);
            client = await factory.GetClientWithTokenAsync(new() { Email = user.Email, Password = user.Password });
            var updateUserDto = new UpdateUserDto()
            {
                UserName = user.Username + "Updated",
                Email = user.Email,
                PhoneNumber = "123321123"
            };
            //Act
            var response = await client.PutAsJsonAsync("/api/v1/account/update-profile", updateUserDto);
            //Assert
            var result = await response.Content.ReadFromJsonAsync<UserTokenResponse>();
            using (new AssertionScope())
            {
                response.EnsureSuccessStatusCode();
                result.Should().NotBeNull();
                result.IsSucceed.Should().BeTrue();
                result.Token.Should().NotBeNullOrWhiteSpace();
            }

            //Clear data
            await _userSqlHelper.RemoveTestAuthenticationUsersAsync(updateUserDto.UserName);
        }
        [Fact]
        public async Task UpdateProfileAsync_TakenEmail_BadRequest()
        {
            //Arrange
            var user = TestAccountUser;

            var existingUser = TestAccountUser;
            existingUser.Username = user.Username + "Existing";
            existingUser.Email = user.Email + "Existing";
            existingUser.Password = user.Password + "Existing";

            var client = factory.CreateClient();
            await client.PostAsJsonAsync("/api/v1/authentication/register", user);
            await client.PostAsJsonAsync("/api/v1/authentication/register", existingUser);
            client = await factory.GetClientWithTokenAsync(new() { Email = user.Email, Password = user.Password });
            var updateUserDto = new UpdateUserDto()
            {
                UserName = user.Username + "Updated",
                Email = user.Email + "Existing",
                PhoneNumber = "123321123"
            };
            //Act
            var response = await client.PutAsJsonAsync("/api/v1/account/update-profile", updateUserDto);
            //Assert
            var result = await response.Content.ReadFromJsonAsync<UserTokenResponse>();
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.IsSucceed.Should().BeFalse();
                result.Token.Should().BeNullOrWhiteSpace();
            }

            //Clear data
            await _userSqlHelper.RemoveTestAuthenticationUsersAsync(user.Username);
            await _userSqlHelper.RemoveTestAuthenticationUsersAsync(existingUser.Username);
        }
    }
}
