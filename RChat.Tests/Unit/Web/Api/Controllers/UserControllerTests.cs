using Microsoft.AspNetCore.Mvc;
using Moq;
using RChat.Domain.Repsonses;
using RChat.Domain.Users.DTO;
using RChat.Domain;
using RChat.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using RChat.Application.Contracts.Users;
using FluentAssertions;

namespace RChat.Tests.Unit.Web.Api.Controllers
{
    public class UserControllerTests
    {
        [Fact]
        public async Task GetUsersInformation_Should_Return_OkResult_With_UsersInformationList()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            var controller = new UserController(userServiceMock.Object);
            var page = 0;
            var size = 1;
            string? value = null;
            string? orderBy = "UserName";
            string? orderByType = "Ascending";

            var expectedSearchArguments = new SearchArguments(value, (page - 1) * size, size, orderBy, orderByType);
            var expectedResult = new GridListDto<UserInformationDto>
            {
                SelectedEntities = new List<UserInformationDto> { new UserInformationDto { Id = 1, UserName = "User1" } },
                TotalCount = 1
            };
            userServiceMock.Setup(service => service.GetUsersInformationListAsync(It.IsAny<SearchArguments>()))
                .ReturnsAsync(expectedResult);
            // Act
            var result = await controller.GetUsersInformation(page, size, value, orderBy, orderByType);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            var actualResult = Assert.IsType<GridListDto<UserInformationDto>>(okResult.Value);
            Assert.Equal(expectedResult.TotalCount, actualResult.TotalCount);
            Assert.Equal(expectedResult.SelectedEntities.Count(), actualResult.SelectedEntities.Count());
            actualResult.SelectedEntities.Should().BeInAscendingOrder(c => c.UserName);
        }
    }
}
