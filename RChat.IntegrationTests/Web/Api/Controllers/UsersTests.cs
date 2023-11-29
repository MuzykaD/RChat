using FluentAssertions;
using FluentAssertions.Execution;
using RChat.Domain.Chats.Dto;
using RChat.Domain.Repsonses;
using RChat.Domain.Users.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace RChat.IntegrationTests.Web.Api.Controllers
{
    public  class UsersTests : TestBase
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
    }
}
