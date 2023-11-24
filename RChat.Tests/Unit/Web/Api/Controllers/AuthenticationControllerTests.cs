using Microsoft.AspNetCore.Mvc;
using Moq;
using RChat.Application.Contracts.Authentication;
using RChat.Domain.Users.DTO;
using RChat.WebApi.Controllers;
using Xunit;

namespace RChat.Tests.Unit.Web.Api.Controllers
{
    public class AuthenticationControllerTests
    {
        [Fact]
        public async Task LoginAsync_ValidData_ReturnsOk()
        {
            //Arrange
            var authenticationServiceMock = new Mock<IAuthenticationService>();
            string token = "dummy";
            authenticationServiceMock.Setup(a => a.GetTokenByCredentials(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(token);
            var loginData = new LoginUserDto() { Email = "dummy@mail.com", Password = "Qwerty123!" };
            var sut = new AuthenticationController(authenticationServiceMock.Object);
            //Act
            var result = await sut.LoginAsync(loginData);
            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task LoginAsync_InvalidData_ReturnsUnauthorized()
        {
            //Arrange
            var authenticationServiceMock = new Mock<IAuthenticationService>();
            string? token = null;
            authenticationServiceMock.Setup(a => a.GetTokenByCredentials(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(token);
            var loginData = new LoginUserDto() { Email = "dummy@mail.com", Password = "Qwerty123!" };
            var sut = new AuthenticationController(authenticationServiceMock.Object);
            //Act
            var result = await sut.LoginAsync(loginData);
            //Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }
        [Fact]
        public async Task RegisterAsync_ValidData_ReturnsOk()
        {
            //Arrange
            var authenticationServiceMock = new Mock<IAuthenticationService>();
            var registerDto = new RegisterUserDto()
            {
                Email = "testemail@mail.com",
                Username = "test",
                Password = "Qwerty123!"
            };
            authenticationServiceMock
                .Setup(a => a.RegisterUserAsync(It.IsAny<RegisterUserDto>()))
                .ReturnsAsync(true);
            var sut = new AuthenticationController(authenticationServiceMock.Object);
            //Act
            var result = await sut.RegisterAsync(registerDto);
            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task RegisterAsync_ExistingUser_ReturnsBadRequest()
        {
            //Arrange
            var authenticationServiceMock = new Mock<IAuthenticationService>();
            var registerDto = new RegisterUserDto()
            {
                Email = "existing@mail.com",
                Username = "test",
                Password = "Qwerty123!"
            };
            authenticationServiceMock
                .Setup(a => a.RegisterUserAsync(It.IsAny<RegisterUserDto>()))
                .ReturnsAsync(false);
            var sut = new AuthenticationController(authenticationServiceMock.Object);
            //Act
            var result = await sut.RegisterAsync(registerDto);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
