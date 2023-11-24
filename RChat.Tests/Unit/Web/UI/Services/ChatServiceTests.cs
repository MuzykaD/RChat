using Moq;
using RChat.Domain.Repsonses;
using RChat.UI.Common.HttpClientPwa.Interfaces;
using RChat.UI.Common;
using RChat.UI.ViewModels.Chat;
using RChat.UI.ViewModels.InformationViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using RChat.UI.Services.ChatService;
using RChat.Domain.Chats.Dto;

namespace RChat.Tests.Unit.Web.UI.Services
{
    public class ChatServiceTests
    {
        [Fact]
        public async Task CreatePublicGroupAsync_Should_Send_PostRequest_To_Correct_Endpoint()
        {
            // Arrange
            var httpClientMock = new Mock<IHttpClientPwa>();
            var chatService = new ChatService(httpClientMock.Object);

            var users = new HashSet<UserInformationViewModel>
        {
            new UserInformationViewModel { Id = 1, UserName = "User1" },
            new UserInformationViewModel { Id = 2, UserName = "User2" },
        };
            var groupName = "TestGroup";

            httpClientMock.Setup(client => client.SendPostRequestAsync<CreateGroupChatDto, ApiResponse>(
                    RChatApiRoutes.ChatsGroup, It.IsAny<CreateGroupChatDto>()))
                .ReturnsAsync(new ApiRequestResult<ApiResponse> { IsSuccessStatusCode = true });

            // Act
            await chatService.CreatePublicGroupAsync(users, groupName);

            // Assert
            httpClientMock.Verify(
                client => client.SendPostRequestAsync<CreateGroupChatDto, ApiResponse>(
                    RChatApiRoutes.ChatsGroup, It.Is<CreateGroupChatDto>(dto =>
                        dto.MembersId.Count == 2 &&
                        dto.GroupName == groupName)),
                Times.Once);
        }

        [Fact]
        public async Task GetGroupChatByIdAsync_Should_Send_GetRequest_To_Correct_Endpoint()
        {
            // Arrange
            var httpClientMock = new Mock<IHttpClientPwa>();
            var chatService = new ChatService(httpClientMock.Object);

            var chatId = 1;
            var expectedEndpoint = $"{RChatApiRoutes.ChatsGroup}/{chatId}";

            httpClientMock.Setup(client => client.SendGetRequestAsync<ChatViewModel>(expectedEndpoint))
                .ReturnsAsync(new ApiRequestResult<ChatViewModel> { IsSuccessStatusCode = true });

            // Act
            await chatService.GetGroupChatByIdAsync(chatId);

            // Assert
            httpClientMock.Verify(
                client => client.SendGetRequestAsync<ChatViewModel>(expectedEndpoint),
                Times.Once);
        }

        [Fact]
        public async Task GetInformationListAsync_Should_Send_GetRequest_To_Correct_Endpoint_With_Query_Parameters()
        {
            // Arrange
            var httpClientMock = new Mock<IHttpClientPwa>();
            var chatService = new ChatService(httpClientMock.Object);

            var page = 1;
            var size = 10;
            string? value = null;
            string? orderBy = "Name";
            string? orderByType = "Ascending";
            httpClientMock.Setup(client => client.SendGetRequestAsync<GridListDto<ChatInformationViewModel>>(It.IsAny<string>()))
                .ReturnsAsync(new ApiRequestResult<GridListDto<ChatInformationViewModel>> { Result = new(), IsSuccessStatusCode = true });

            // Act
            var result = await chatService.GetInformationListAsync(page, size, value, orderBy, orderByType);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ApiRequestResult<GridListDto<ChatInformationViewModel>>>(result);
            Assert.IsType<GridListDto<ChatInformationViewModel>>(result.Result);
            Assert.True(result.IsSuccessStatusCode);
            httpClientMock.Verify(
                client => client.SendGetRequestAsync<GridListDto<ChatInformationViewModel>>(It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public async Task GetPrivateChatByUserIdAsync_Should_Send_GetRequest_To_Correct_Endpoint()
        {
            // Arrange
            var httpClientMock = new Mock<IHttpClientPwa>();
            var chatService = new ChatService(httpClientMock.Object);

            var userId = 1;
            var expectedEndpoint = $"{RChatApiRoutes.ChatsPrivate}/{userId}";

            httpClientMock.Setup(client => client.SendGetRequestAsync<ChatViewModel>(expectedEndpoint))
                .ReturnsAsync(new ApiRequestResult<ChatViewModel> { Result = new(), IsSuccessStatusCode = true });

            // Act
            var result = await chatService.GetPrivateChatByUserIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ApiRequestResult<ChatViewModel>>(result);
            Assert.IsType<ChatViewModel>(result.Result);
            Assert.True(result.IsSuccessStatusCode);
            httpClientMock.Verify(
                client => client.SendGetRequestAsync<ChatViewModel>(expectedEndpoint),
                Times.Once);
        }
    }
}
