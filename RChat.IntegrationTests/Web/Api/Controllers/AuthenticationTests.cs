using Azure;
using FluentAssertions;
using FluentAssertions.Execution;
using RChat.Domain.Repsonses;
using RChat.Domain.Users.DTO;
using System.Net.Http.Json;


namespace RChat.IntegrationTests.Web.Api.Controllers
{
    [Collection("RChat_Sequence")]
    public class AuthenticationTests : TestBase, IAsyncLifetime
    {      
        [Fact]
        public async Task Register_ValidData_UserRegistered_OkResult()
        {
            //Arrange
            var client = factory.CreateClient();
            var registerDto = new RegisterUserDto()
            {
                Username = "Test4",
                Email = "test4@mail.com",
                Password = "Test4!"
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
        }

        [Fact]
        public async Task Register_ExistingEmail_UserNotRegistered_BadRequestResult()
        {
            //Arrange
            var client = factory.CreateClient();
            var registerDto = new RegisterUserDto()
            {
                Username = "anotherAdmin",
                Email = "admin@mail.com",
                Password = "Admin1!"
            };
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
        }

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

        public async Task InitializeAsync()
        {
            await ClearTables();
            await SeedUsersDataAsync();
        }

        public async Task DisposeAsync()
        {
            await ClearTables();
        }
    }
}
