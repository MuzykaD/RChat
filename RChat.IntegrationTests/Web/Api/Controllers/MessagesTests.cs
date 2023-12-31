﻿
using RChat.Domain.Messages.Dto;
using RChat.Domain.Repsonses;
using System.Net.Http.Json;
using Xunit.Sdk;

namespace RChat.IntegrationTests.Web.Api.Controllers
{
    [Collection("RChat_Sequence")]
    public class MessagesTests : TestBase, IAsyncLifetime
    {

        [Fact]
        public async Task DeleteMessageAsync_ValidMessageData_ShouldDeleteMessage()
        {
            //Arrange
            int testMessageId = 1;
            var client = await factory.GetClientWithTokenAsync();
            //Act
            var response = await client.DeleteAsync($"/api/v1/messages?messageId={testMessageId}");
            //Assert
            var messageExists = await CheckIfRecordExists("Messages", "Id", testMessageId);
            using (new AssertionScope())
            {
                response.EnsureSuccessStatusCode();
                messageExists.Should().BeFalse();
            }

        }

        [Fact]
        public async Task DeleteMessageAsync_WrongSenderId_ReturnsBadRequest()
        {
            //Arrange
            int testMessageId = 2;
            var client = await factory.GetClientWithTokenAsync();
            //Act
            var response = await client.DeleteAsync($"/api/v1/messages?messageId={testMessageId}");
            //Assert
            var messageExists = await CheckIfRecordExists("Messages", "Id", testMessageId);
            using (new AssertionScope())
            {
                response.IsSuccessStatusCode.Should().BeFalse();
                messageExists.Should().BeTrue();
            }
        }
        [Fact]
        public async Task CreateMessageAsync_ValidSenderId_MessageCreated()
        {
            var messageToCreate = new MessageInformationDto() { Content = "MessageToCreateInTest", ChatId = 2, SentAt = DateTime.Now };
            var client = await factory.GetClientWithTokenAsync();
            //Act
            var response = await client.PostAsJsonAsync($"/api/v1/messages", messageToCreate);
            //Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result!.Message.Should().NotBeNullOrWhiteSpace();
            }
        }

        [Fact]
        public async Task UpdateMessageAsync_ValidData_MessageUpdated_OkResult()
        {
            var messageToUpdate = new MessageInformationDto() { Id = 3, Content = "UPDATED!", SenderId = 999, ChatId = 2 };
            var client = await factory.GetClientWithTokenAsync();
            //Act
            var response = await client.PutAsJsonAsync($"/api/v1/messages", messageToUpdate);
            //Assert           
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
            var messageWithOldContentExists = await CheckIfRecordExists("Messages", "Content", "MessageToUpdateByUser");
            var messageWithNewContentExists = await CheckIfRecordExists("Messages", "Content", "UPDATED!");
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.IsSucceed.Should().BeTrue();
                messageWithNewContentExists.Should().BeTrue();
                messageWithOldContentExists.Should().BeFalse();
            }
        }

        public async Task InitializeAsync()
        {
            await ClearTables();
            await SeedUsersDataAsync();
            await SeedChatsDataAsync();
            await SeedChatUsersDataAsync();
            await SeedMessagesDataAsync();
        }

        public async Task DisposeAsync()
        {
            await ClearTables();
        }
    }
}
