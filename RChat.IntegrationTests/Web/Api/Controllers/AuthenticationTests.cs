using Azure;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.DependencyInjection;
using RChat.Domain.Repsonses;
using RChat.Domain.Users.DTO;
using RChat.Infrastructure.Contracts.UnitOfWork;
using RChat.IntegrationTests.Configuration;
using RChat.IntegrationTests.DbSqlHelpers.UserDbSqlHelper;
using RChat.IntegrationTests.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace RChat.IntegrationTests.Web.Api.Controllers
{
    public class AuthenticationTests : TestBase, IClassFixture<AuthenticationFixture>
    {
        private UserDbSqlHelper _userDbSqlHelper;
        public AuthenticationTests()
        {
            _userDbSqlHelper = new UserDbSqlHelper();
        }
        [Fact]
        public async Task Register_ValidData_UserRegistered_OkResult()
        {
            //Arrange
            var client = factory.CreateClient();
            var registerDto = new RegisterUserDto()
            {
                Username = "RegisterUserTest",
                Email = "registerUserTest@mail.com",
                Password = "RegisterUser1!"
            };
            //Act
            var response = await client.PostAsJsonAsync("/api/v1/authentication/register", registerDto);
            //Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Should().BeOfType<ApiResponse>();
                result.IsSucceed.Should().BeTrue();
                result.Message.Should().Be("Account created! You can use credentials to log in!");
            }
            await _userDbSqlHelper.RemoveTestAuthenticationUsersAsync(registerDto.Username);
        }

        [Fact]
        public async Task Register_ExistingEmail_UserNotRegistered_BadRequestResult()
        {
            //Arrange
            var client = factory.CreateClient();
            var registerDto = new RegisterUserDto()
            {
                Username = "ExistingTestUser",
                Email = "existingUserTest@mail.com",
                Password = "ExistingUser1!"
            };
            await _userDbSqlHelper.InsertTestUserAsync(registerDto);
            //Act
            var response = await client.PostAsJsonAsync("/api/v1/authentication/register", registerDto);
            //Assert
            var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
            using(new AssertionScope())
            {
                response.IsSuccessStatusCode.Should().BeFalse();
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
                result.IsSucceed.Should().BeFalse();
                result.Message.Should().Be("Account with such email already exists, try another one!");
            }
            await _userDbSqlHelper.RemoveTestAuthenticationUsersAsync(registerDto.Username);
        }

        // this user is always present in test-db
        [Fact]
        public async Task Login_ValidData_ReturnsToken()
        {
            //Arrange
            var client = factory.CreateClient();         
            var login = new LoginUserDto()
            {
                Email = "admin@mail.com",
                Password = "Admin1!"
            };
            //Act
            var response = await client.PostAsJsonAsync("/api/v1/authentication/login", login);
            //Assert
            response.EnsureSuccessStatusCode();;
            var result = await response.Content.ReadFromJsonAsync<UserTokenResponse>();
            using (new AssertionScope())
            {
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
                result.Should().NotBeNull();
                result.Should().BeOfType<UserTokenResponse>();
                result.Token.Should().NotBeNullOrWhiteSpace();
            }
        }

        [Fact]
        public async Task Login_InvalidData_ReturnsUnauthorized()
        {
            //Arrange
            var client = factory.CreateClient();
            var login = new LoginUserDto()
            {
                Email = "dummy@mail.com",
                Password = "Dummy!!"
            };
            //Act
            var response = await client.PostAsJsonAsync("/api/v1/authentication/login", login);
            //Assert
            var result = await response.Content.ReadFromJsonAsync<UserTokenResponse>();
            using (new AssertionScope())
            {
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
                result.Should().NotBeNull();
                result.Should().BeOfType<UserTokenResponse>();
                result.Token.Should().BeNullOrEmpty();
            }
        }
    }
}
