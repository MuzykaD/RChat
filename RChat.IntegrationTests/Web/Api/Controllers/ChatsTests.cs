using Azure;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Dto;
using RChat.Domain.Repsonses;
using RChat.Domain.Users.DTO;
using RChat.Infrastructure.Context;
using RChat.Infrastructure.Contracts.UnitOfWork;
using RChat.Infrastructure.UnitOfWork;
using RChat.IntegrationTests.Configuration;
using RChat.IntegrationTests.DbSqlHelpers.UserDbSqlHelper;
using RChat.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace RChat.IntegrationTests.Web.Api.Controllers
{
    public class ChatsTests : TestBase
    {
        private ChatDbSqlHelper _chatDbSqlHelper = new ChatDbSqlHelper();

        [Fact]
        public async Task GetChatsInformation_Should_Return_OkResult_With_ChatList()
        {
            //Arrange
            var client = await factory.GetClientWithTokenAsync();           
            //Act
            var response = await client.GetAsync("/api/v1/chats?page=0&size=1");
            //Assert
            response.EnsureSuccessStatusCode();
            var resultData = await response.Content.ReadFromJsonAsync<GridListDto<ChatInformationDto>>();
            using(new AssertionScope())
            {
                resultData.Should().NotBeNull();
                resultData.Should().BeOfType<GridListDto<ChatInformationDto>>();
            }
        }
        [Fact]
        public async Task GetPrivateChatByEmailAsync_ReturnsPrivateChat()
        {
            //todo
            int testUserId = 9;
            //Arrange
            var client = await factory.GetClientWithTokenAsync();
            //Act
            var resposne = await client.GetAsync($"/api/v1/chats/private/{testUserId}");
            //Assert
            resposne.EnsureSuccessStatusCode();
            var resultData = await resposne.Content.ReadFromJsonAsync<ChatDto>();
            using(new AssertionScope())
            {
                resultData.Should().NotBeNull();
                resultData.Should().BeOfType<ChatDto>();
                resultData.IsGroupChat.Should().BeFalse();
                resultData.Users.Should().Contain(user => user.Id == testUserId);
            }
        }
        [Fact]
        public async Task GetGroupChatByIdAsync_ReturnsValidChat()
        {
            int testChatId = 1;
            var client = await factory.GetClientWithTokenAsync();
            //Act
            var response = await client.GetAsync($"/api/v1/chats/group/{testChatId}");
            //Assert

            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ChatDto>();
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Should().BeOfType<ChatDto>();
                result.Id.Should().Be(testChatId);

            }
        }
        [Fact]
        public async Task CreateGroupAsync_GroupCreated()
        {
            //Arrange
            var createGroupDto = new CreateGroupChatDto()
            {
                GroupName = "CreatedGroupChatTest",
                MembersId = new int[] { 1 }
            };
            var client = await factory.GetClientWithTokenAsync();

            //Act
            var response = await client.PostAsJsonAsync($"/api/v1/chats/group", createGroupDto);
            //Assert
            response.EnsureSuccessStatusCode();
            await _chatDbSqlHelper.DeleteTestChatDataAsync();

        }

        [Fact]
        public async Task GetUserGroupsInfo_ReturnListOfIdentifiers()
        {
            //Arrange
            var client = await factory.GetClientWithTokenAsync();
            //Act
            var response = await client.GetAsync($"/api/v1/chats/group-info");
            //Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<GroupsIdentifies>();
            using( new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Should().BeOfType<GroupsIdentifies>();
                result.IsSucceed.Should().BeTrue();
            }          
        }
    }
}
