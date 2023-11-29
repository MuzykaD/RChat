
using RChat.Domain.Repsonses;
using RChat.Domain.Users.DTO;
using System.Net.Http.Json;


namespace RChat.IntegrationTests.Web.Api.Controllers
{
    [Collection("RChat_Sequence")]
    public  class UsersTests : TestBase, IAsyncLifetime
    {      
        [Fact]
        public async Task GetUsersInformation_Should_Return_OkResult_With_UsersList()
        {
            //Arrange
            var client = await factory.GetClientWithTokenAsync();
            //Act
            var response = await client.GetAsync("/api/v1/users?page=0&size=1");
            //Assert
            response.EnsureSuccessStatusCode();
            var resultData = await response.Content.ReadFromJsonAsync<GridListDto<UserInformationDto>>();
            using (new AssertionScope())
            {
                resultData.Should().NotBeNull();
                resultData.Should().BeOfType<GridListDto<UserInformationDto>>();
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
