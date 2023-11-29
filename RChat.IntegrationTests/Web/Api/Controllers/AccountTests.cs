using FluentAssertions;
using FluentAssertions.Execution;
using RChat.Domain.Users.DTO;
using System.Net.Http.Json;


namespace RChat.IntegrationTests.Web.Api.Controllers
{
    [Collection("RChat_Sequence")]
    public class AccountTests : TestBase, IAsyncLifetime
    {              
        [Fact]
        public async Task GetPersonalInformationAsync_ReturnsValidProfileData()
        {
            //Arrange            
            var client = await factory.GetClientWithTokenAsync(new() { Email = "test2@mail.com", Password = "Test2!" });
            //Act
            var response = await client.GetAsync("/api/v1/account/profile");
            //Assert
            var result = await response.Content.ReadFromJsonAsync<UserInformationDto>();
            using(new AssertionScope())
            {
                response.EnsureSuccessStatusCode();
                result.Should().NotBeNull();
                result.Id.Should().Be(2);
                result.UserName.Should().Be("Test2");
                result.Email.Should().Be("test2@mail.com");
            }
        }

        [Fact]
        public async Task UpdateProfileAsync_ValidData_ReturnsTrue_ReturnsOkResult()
        {
            //Arrange
            var client = await factory.GetClientWithTokenAsync(new() { Email = "test2@mail.com", Password = "Test2!" });
            var updateUserDto = new UpdateUserDto()
            {
                UserName = "Test2Updated",
                Email = "test2Updated@mail.com",
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
        }
        [Fact]
        public async Task UpdateProfileAsync_TakenEmail_BadRequest()
        {
            //Arrange
            var client = await factory.GetClientWithTokenAsync(new() { Email = "test2@mail.com", Password = "Test2!" });
            var updateUserDto = new UpdateUserDto()
            {
                UserName = "Test2Updated",
                Email = "test1@mail.com", //existing email
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
