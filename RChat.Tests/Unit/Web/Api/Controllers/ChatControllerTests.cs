using Microsoft.AspNetCore.Mvc;
using Moq;
using RChat.Domain.Chats.Dto;
using RChat.Domain.Repsonses;
using RChat.Domain;
using RChat.WebApi.Controllers;
using Xunit;
using RChat.Application.Contracts.Chats;
using Microsoft.AspNetCore.Http;
using RChat.Domain.Chats;
using System.Security.Claims;

namespace RChat.Tests.Unit.Web.Api.Controllers
{
    public class ChatControllerTests
    {

        [Fact]
        public async Task GetChatsInformation_Should_Return_OkResult_With_ChatList()
        {
            // Arrange
            var chatServiceMock = new Mock<IChatService>();
            var controller = new ChatController(chatServiceMock.Object);
            var page = 0;
            var size = 1;
            string? value = null;
            string? orderBy = null;
            string? orderByType = null;          
            var expectedResult = new GridListDto<ChatInformationDto>
            {
                SelectedEntities = new List<ChatInformationDto> { new ChatInformationDto { Id = 1, Name = "Group1" } },
                TotalCount = 1
            };
            chatServiceMock.Setup(service => service.GetChatsInformationListAsync(It.IsAny<SearchArguments>()))
                .ReturnsAsync(expectedResult);
            // Act
            var result = await controller.GetChatsInformation(page, size, value, orderBy, orderByType);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualResult = Assert.IsType<GridListDto<ChatInformationDto>>(okResult.Value);
            Assert.Equal(expectedResult.TotalCount, actualResult.TotalCount);
            Assert.Equal(expectedResult.SelectedEntities.Count(), actualResult.SelectedEntities.Count());
        }
        [Fact]
        public async Task GetPrivateChatByEmailAsync_Should_Return_OkResult_With_ChatDto()
        {
            // Arrange
            var chatServiceMock = new Mock<IChatService>();
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "1") };
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));
            var controller = new ChatController(chatServiceMock.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };
            var userId = 2;
            var currentUserId = 1;

            var expectedChat = new Chat { Id = 1, Name = "PrivateChat" };
            var expectedChatDto = new ChatDto { Id = 1, Name = "PrivateChat" };

            chatServiceMock.Setup(service => service.GetPrivateChatByUsersIdAsync(currentUserId, userId))
                .ReturnsAsync(expectedChat);
            // Act
            var result = await controller.GetPrivateChatByEmailAsync(userId);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            var actualChatDto = Assert.IsType<ChatDto>(okResult.Value);
            Assert.Equal(expectedChatDto.Id, actualChatDto.Id);
            Assert.Equal(expectedChatDto.Name, actualChatDto.Name);
        }

        [Fact]
        public async Task GetGroupChatByIdAsync_Should_Return_OkResult_With_ChatDto()
        {
            // Arrange
            var chatServiceMock = new Mock<IChatService>();
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "1") };
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));                     
            var controller = new ChatController(chatServiceMock.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };
            var chatId = 1;
            var currentUserId = 1;
            var expectedChat = new Chat { Id = 1, Name = "GroupChat" };
            var expectedChatDto = new ChatDto { Id = 1, Name = "GroupChat" };
            chatServiceMock.Setup(service => service.GetGroupChatByIdAsync(currentUserId, chatId))
                .ReturnsAsync(expectedChat);
            // Act
            var result = await controller.GetGroupChatByIdAsync(chatId);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            var actualChatDto = Assert.IsType<ChatDto>(okResult.Value);
            Assert.Equal(expectedChatDto.Id, actualChatDto.Id);
            Assert.Equal(expectedChatDto.Name, actualChatDto.Name);
        }

        [Fact]
        public async Task CreatePublicGroupAsync_Should_Return_OkResult_With_ApiResponse()
        {
            // Arrange
            var chatServiceMock = new Mock<IChatService>();
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "1") };
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));
            var controller = new ChatController(chatServiceMock.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };
            var createGroupDto = new CreateGroupChatDto
            {
                GroupName = "TestGroup",
                MembersId = new[] { 2, 3 }
            };
            var currentUserId = 1;
            chatServiceMock.Setup(service => service.CreatePublicGroupAsync(createGroupDto.GroupName, currentUserId, It.IsAny<List<int>>()))
                .ReturnsAsync(true);
            // Act
            var result = await controller.CreatePublicGroupAsync(createGroupDto);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            var apiResponse = Assert.IsType<ApiResponse>(okResult.Value);
            Assert.True(apiResponse.IsSucceed);
        }

        [Fact]
        public async Task CreatePublicGroupAsync_Should_Return_BadRequestResult_With_ApiResponse()
        {
            // Arrange
            var chatServiceMock = new Mock<IChatService>();
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "1") };
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));
            var controller = new ChatController(chatServiceMock.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };
            var createGroupDto = new CreateGroupChatDto
            {
                GroupName = "TestGroup",
                MembersId = new[] { 2, 3 }
            };
            var currentUserId = 1;

            chatServiceMock.Setup(service => service.CreatePublicGroupAsync(createGroupDto.GroupName, currentUserId, It.IsAny<List<int>>()))
                .ReturnsAsync(false);
            // Act
            var result = await controller.CreatePublicGroupAsync(createGroupDto);
            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
            var apiResponse = Assert.IsType<ApiResponse>(badRequestResult.Value);
            Assert.False(apiResponse.IsSucceed);
        }

        [Fact]
        public async Task GetUserGroupsInfo_Should_Return_OkResult_With_GroupIdentifiers()
        {
            // Arrange
            var chatServiceMock = new Mock<IChatService>();
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "1") };
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));          
            var controller = new ChatController(chatServiceMock.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };
            var currentUserId = 1;

            var expectedGroupIds = new List<int> { 2, 3 };

            chatServiceMock.Setup(service => service.GetGroupsIdentifiersAsync(currentUserId))
                .ReturnsAsync(expectedGroupIds);
            // Act
            var result = await controller.GetUserGroupsInfo();
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            var groupsIdentifiers = Assert.IsType<GroupsIdentifies>(okResult.Value);
            Assert.Equal(expectedGroupIds, groupsIdentifiers.SignalIdentifiers);
            Assert.True(groupsIdentifiers.IsSucceed);
        }
    }
}
