using FluentAssertions;
using FluentAssertions.Execution;
using RChat.Domain.Messages.Dto;
using RChat.Domain.Repsonses;
using RChat.IntegrationTests.DbSqlHelpers.MessageDbSqlHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace RChat.IntegrationTests.Web.Api.Controllers
{
    public class MessagesTests : TestBase
    {
        private MessageDbSqlHelper _messageSqlHelper;
        public MessagesTests()
        {
            _messageSqlHelper = new MessageDbSqlHelper();
        }

        [Fact]
        public async Task DeleteMessageAsync_ValidMessageData_ShouldDeleteMessage()
        {
            //Arrange
            var messageToDelete = new MessageInformationDto() { Content = "MessageToDelete", SenderId = 7, ChatId = 3, SentAt = DateTime.Now };
            var id = await _messageSqlHelper.InsertMessageAndGetIdAsync(messageToDelete);
            var client = await factory.GetClientWithTokenAsync();
            //Act
            var response = await client.DeleteAsync($"/api/v1/messages?messageId={id}");
            //Assert
            response.EnsureSuccessStatusCode();

        }

        [Fact]
        public async Task DeleteMessageAsync_WrongSenderId_ReturnsBadRequest()
        {
            //Arrange
            var messageToDelete = new MessageInformationDto() { Content = "MessageToDelete", SenderId = 9, ChatId = 3, SentAt = DateTime.Now };
            var id = await _messageSqlHelper.InsertMessageAndGetIdAsync(messageToDelete);
            var client = await factory.GetClientWithTokenAsync();
            //Act
            var response = await client.DeleteAsync($"/api/v1/messages?messageId={id}");
            //Assert
            Assert.False(response.IsSuccessStatusCode);

            await _messageSqlHelper.RemoveTestMessageByIdAsync(id);
        }
        [Fact]
        public async Task CreateMessageAsync_ValidSenderId_MessageCreated()
        {
            var messageToCreate = new MessageInformationDto() { Content = "MessageToCreateInTest", SenderId = 7, ChatId = 3, SentAt = DateTime.Now };
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

            await _messageSqlHelper.RemoveTestMessageByIdAsync(int.Parse(result.Message));
        }

        [Fact]
        public async Task UpdateMessageAsync_ValidData_MessageUpdated_OkResult()
        {
            var messageToUpdate = new MessageInformationDto() { Content = "MessageToUpdateInTest", SenderId = 7, ChatId = 3, SentAt = DateTime.Now };
            var messageId = await _messageSqlHelper.InsertMessageAndGetIdAsync(messageToUpdate);
            messageToUpdate.Id = messageId;
            messageToUpdate.Content = "Message was updated!";

            var client = await factory.GetClientWithTokenAsync();
            //Act
            var response = await client.PutAsJsonAsync($"/api/v1/messages", messageToUpdate);
            //Assert           
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.IsSucceed.Should().BeTrue();
            }


            await _messageSqlHelper.RemoveTestMessageByIdAsync(messageId);
        }
    }
}
