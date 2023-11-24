using Moq;
using RChat.Domain.Messages.Dto;
using RChat.Domain.Repsonses;
using RChat.UI.Common.HttpClientPwa.Interfaces;
using RChat.UI.Common;
using RChat.UI.ViewModels.InformationViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using RChat.UI.Services.MessageService;

namespace RChat.Tests.Unit.Web.UI.Services
{
    public class MessageServiceTests
    {
        [Fact]
        public async Task DeleteMessageByIdAsync_Should_Send_DeleteRequest_To_Correct_Endpoint()
        {
            // Arrange
            var httpClientMock = new Mock<IHttpClientPwa>();
            var messageService = new MessageService(httpClientMock.Object);

            var messageId = 1;
            var expectedEndpoint = $"{RChatApiRoutes.Messages}?messageId={messageId}";

            httpClientMock.Setup(client => client.SendDeleteRequestAsync(expectedEndpoint));
            // Act
            await messageService.DeleteMessageByIdAsync(messageId);

            // Assert
            httpClientMock.Verify(
                client => client.SendDeleteRequestAsync(expectedEndpoint),
                Times.Once);
        }

        [Fact]
        public async Task GetInformationListAsync_Should_Send_GetRequest_To_Correct_Endpoint_With_Query_Parameters()
        {
            // Arrange
            var httpClientMock = new Mock<IHttpClientPwa>();
            var messageService = new MessageService(httpClientMock.Object);

            var page = 1;
            var size = 10;
            string? value = "value";
            string? orderBy = "Content";
            string? orderByType = "Descending";
            string? expectedEndpoint = $"{RChatApiRoutes.Messages}?page={page}&size={size}&value={value}&orderBy={orderBy}&orderByType={orderByType}";

            httpClientMock.Setup(client => client.SendGetRequestAsync<GridListDto<MessageInformationViewModel>>(expectedEndpoint))
                .ReturnsAsync(new ApiRequestResult<GridListDto<MessageInformationViewModel>> {Result = new(), IsSuccessStatusCode = true });

            // Act
           var result =  await messageService.GetInformationListAsync(page, size, value, orderBy, orderByType);

            // Assert
            Assert.IsType<ApiRequestResult<GridListDto<MessageInformationViewModel>>>(result);
            Assert.True(result.IsSuccessStatusCode);
            Assert.IsType<GridListDto<MessageInformationViewModel>>(result.Result);
            httpClientMock.Verify(
                client => client.SendGetRequestAsync<GridListDto<MessageInformationViewModel>>(expectedEndpoint),
                Times.Once);
        }

        [Fact]
        public async Task SendMessageAsync_Should_Send_PostRequest_To_Correct_Endpoint()
        {
            // Arrange
            var httpClientMock = new Mock<IHttpClientPwa>();
            var messageService = new MessageService(httpClientMock.Object);

            var messageDto = new MessageInformationDto();
            var expectedResult = new ApiRequestResult<ApiResponse> { IsSuccessStatusCode = true, Result = new ApiResponse { Message = "123" } };

            httpClientMock.Setup(client => client.SendPostRequestAsync<MessageInformationDto, ApiResponse>(
                    RChatApiRoutes.Messages, messageDto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await messageService.SendMessageAsync(messageDto);

            // Assert
            Assert.Equal(123, result);
            httpClientMock.Verify(
                client => client.SendPostRequestAsync<MessageInformationDto, ApiResponse>(
                    RChatApiRoutes.Messages, messageDto),
                Times.Once);
        }

        [Fact]
        public async Task UpdateMessageAsync_Should_Send_PutRequest_To_Correct_Endpoint()
        {
            // Arrange
            var httpClientMock = new Mock<IHttpClientPwa>();
            var messageService = new MessageService(httpClientMock.Object);

            var messageToUpdate = new MessageInformationDto();
            var expectedResult = new ApiRequestResult<ApiResponse> { IsSuccessStatusCode = true };

            httpClientMock.Setup(client => client.SendPutRequestAsync<MessageInformationDto, ApiResponse>(
                    RChatApiRoutes.Messages, messageToUpdate))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await messageService.UpdateMessageAsync(messageToUpdate);

            // Assert
            Assert.Same(expectedResult, result);
            httpClientMock.Verify(
                client => client.SendPutRequestAsync<MessageInformationDto, ApiResponse>(
                    RChatApiRoutes.Messages, messageToUpdate),
                Times.Once);
        }
    }
}
