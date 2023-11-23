using Microsoft.AspNetCore.Mvc;
using Moq;
using RChat.Domain.Messages.Dto;
using RChat.Domain.Repsonses;
using RChat.Domain;
using RChat.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using RChat.Application.Contracts.Messages;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace RChat.Tests.Unit.Web.Api.Controllers
{
    public class MessageControllerTests
    {
        [Fact]
        public async Task GetUsersInformation_Should_Return_OkResult_With_MessagesInformationList()
        {
            // Arrange
            var messageServiceMock = new Mock<IMessageService>();
            var controller = new MessageController(messageServiceMock.Object);
            var page = 0;
            var size = 1;
            string? value = null;
            string? orderBy = "Content";
            string? orderByType = "Descending";
            var expectedSearchArguments = new SearchArguments(value, (page - 1) * size, size, orderBy, orderByType);
            var expectedResult = new GridListDto<MessageInformationDto>
            {
                SelectedEntities = new List<MessageInformationDto> { new MessageInformationDto { Id = 1, Content = "Hello" } },
                TotalCount = 1
            };
            messageServiceMock.Setup(service => service.GetMessagesInformationListAsync(It.IsAny<SearchArguments>()))
                .ReturnsAsync(expectedResult);
            // Act
            var result = await controller.GetUsersInformation(page, size, value, orderBy, orderByType);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            var actualResult = Assert.IsType<GridListDto<MessageInformationDto>>(okResult.Value);
            Assert.Equal(expectedResult.TotalCount, actualResult.TotalCount);
            Assert.Equal(expectedResult.SelectedEntities.Count(), actualResult.SelectedEntities.Count());
            actualResult.SelectedEntities.Should().BeInDescendingOrder(s => s.Content);
        }

        [Fact]
        public async Task CreateMessageAsync_Should_Return_OkResult_With_ApiResponse()
        {
            // Arrange
            var messageServiceMock = new Mock<IMessageService>();
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "1") };
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));
            
            var controller = new MessageController(messageServiceMock.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };
            var userId = 1;
            var messageDto = new MessageInformationDto { Content = "Hello" };

            messageServiceMock.Setup(service => service.CreateMessageAsync(userId, messageDto))
                .ReturnsAsync(1);
            // Act
            var result = await controller.CreateMessageAsync(messageDto);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            Assert.IsType<ApiResponse>(okResult.Value);

        }

        [Fact]
        public async Task DeleteMessageAsync_Should_Return_OkResult_When_DeletionSucceeds()
        {
            // Arrange
            var messageServiceMock = new Mock<IMessageService>();
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "2") };
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            var controller = new MessageController(messageServiceMock.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };
            var messageId = 1;
            var userId = 2;
            messageServiceMock.Setup(service => service.DeleteMessageAsync(messageId, userId))
                .ReturnsAsync(true);
            // Act
            var result = await controller.DeleteMessageAsync(messageId);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            var apiResponse = Assert.IsType<bool>(okResult.Value);
            Assert.True(apiResponse);
        }

        [Fact]
        public async Task DeleteMessageAsync_Should_Return_BadRequestResult_When_DeletionFails()
        {
            // Arrange
            var messageServiceMock = new Mock<IMessageService>();
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "1") };
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));
            var controller = new MessageController(messageServiceMock.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };
            var messageId = 1;
            var userId = 2;
            messageServiceMock.Setup(service => service.DeleteMessageAsync(messageId, userId))
                .ReturnsAsync(false);
            // Act
            var result = await controller.DeleteMessageAsync(messageId);
            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
            var apiResponse = Assert.IsType<bool>(badRequestResult.Value);
            Assert.False(apiResponse);
        }

        [Fact]
        public async Task UpdateMessageAsync_Should_Return_OkResult_When_UpdateSucceeds()
        {
            // Arrange
            var messageServiceMock = new Mock<IMessageService>();
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "1") };
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));
            var controller = new MessageController(messageServiceMock.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };
            var messageDto = new MessageInformationDto { Id = 1, Content = "Updated content" };
            var currentUserId = 1;
            messageServiceMock.Setup(service => service.UpdateMessageAsync(currentUserId, messageDto))
                .ReturnsAsync(true);
            // Act
            var result = await controller.UpdateMessageAsync(messageDto);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            var apiResponse = Assert.IsType<ApiResponse>(okResult.Value);
            Assert.True(apiResponse.IsSucceed);
        }

        [Fact]
        public async Task UpdateMessageAsync_Should_Return_BadRequestResult_When_UpdateFails()
        {
            // Arrange
            var messageServiceMock = new Mock<IMessageService>();
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "1") };
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));
            var controller = new MessageController(messageServiceMock.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };
            var messageDto = new MessageInformationDto { Id = 1, Content = "Updated content" };
            var currentUserId = 1;
            messageServiceMock.Setup(service => service.UpdateMessageAsync(currentUserId, messageDto))
               .ReturnsAsync(false);
            // Act
            var result = await controller.UpdateMessageAsync(messageDto);
            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
            var apiResponse = Assert.IsType<ApiResponse>(badRequestResult.Value);
            Assert.False(apiResponse.IsSucceed);
        }
    }
}
