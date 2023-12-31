﻿using Azure;

using RChat.Domain.Chats.Dto;
using RChat.Domain.Repsonses;
using System.Net.Http.Json;


namespace RChat.IntegrationTests.Web.Api.Controllers
{
    [Collection("RChat_Sequence")]
    public class ChatsTests : TestBase, IAsyncLifetime
    {

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
            using (new AssertionScope())
            {
                resultData.Should().NotBeNull();
                resultData.Should().BeOfType<GridListDto<ChatInformationDto>>();
            }
        }
        [Fact]
        public async Task GetPrivateChatByEmailAsync_ReturnsPrivateChat()
        {
            //todo
            int testUserId = 1;
            //Arrange
            var client = await factory.GetClientWithTokenAsync();
            //Act
            var resposne = await client.GetAsync($"/api/v1/chats/private/{testUserId}");
            //Assert
            resposne.EnsureSuccessStatusCode();
            var resultData = await resposne.Content.ReadFromJsonAsync<ChatDto>();
            using (new AssertionScope())
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
            int testChatId = 2;
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
                MembersId = new int[] { 1, 2 }
            };
            var client = await factory.GetClientWithTokenAsync();
            //Act
            var response = await client.PostAsJsonAsync($"/api/v1/chats/group", createGroupDto);
            //Assert
            var chatExists = await CheckIfRecordExists("Chats", "Name", createGroupDto.GroupName);
            using (new AssertionScope())
            {
                response.EnsureSuccessStatusCode();
                chatExists.Should().BeTrue();
            }
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
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Should().BeOfType<GroupsIdentifies>();
                result.IsSucceed.Should().BeTrue();
            }
        }

        public async Task InitializeAsync()
        {
            await ClearTables();
            await SeedUsersDataAsync();
            await SeedChatsDataAsync();
            await SeedChatUsersDataAsync();
        }

        public async Task DisposeAsync()
        {
            await ClearTables();
        }
    }
}
