using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RChat.Application.Contracts.Account;
using RChat.Application.Contracts.Authentication.JWT;
using RChat.Domain.Users.DTO;
using RChat.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RChat.Tests.Unit.Web.Api.Controllers
{
    public class AccountControllerTests
    {
        #region ChangePasswordAsync
        [Fact]
        public async Task ChangePasswordAsync_ValidData_ReturnsOk()
        {
            //Arrange
            var jwtServiceMock = new Mock<IJwtTokenService>();
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(a =>
                                     a.ChangeUserPasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                              .ReturnsAsync(true);
            var changePasswordDto = new ChangeUserPasswordDto()
            {
                CurrentPassword = "current",
                NewPassword = "newPassword"
            };
            var claims = new ClaimsIdentity(new List<Claim>() { new Claim(ClaimTypes.Email, "test@mail.com") });
            var sut = new AccountController(accountServiceMock.Object, jwtServiceMock.Object);
            sut.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(claims)
            };
            //Act
            var result = await sut.ChangePasswordAsync(changePasswordDto);
            //Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task ChangePasswordAsync_InvalidData_ReturnsBadRequest()
        {
            //Arrange
            var jwtServiceMock = new Mock<IJwtTokenService>();
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(a =>
                                     a.ChangeUserPasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                              .ReturnsAsync(false);
            var changePasswordDto = new ChangeUserPasswordDto()
            {
                CurrentPassword = "current",
                NewPassword = "newPassword"
            };
            var claims = new ClaimsIdentity(new List<Claim>() { new Claim(ClaimTypes.Email, "test@mail.com") });
            
            var sut = new AccountController(accountServiceMock.Object, jwtServiceMock.Object);
            sut.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(claims)
            };
            //Act
            var result = await sut.ChangePasswordAsync(changePasswordDto);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        #endregion
        #region GetPersonalInformationAsync
        [Fact]
        public async Task GetPersonalInformationAsync_ReturnsOk()
        {
            //Arrange
            var jwtServiceMock = new Mock<IJwtTokenService>();
            var accountServiceMock = new Mock<IAccountService>();
            var userInfoDto = new UserInformationDto()
            {
                Id = 1,
                UserName = "test",
                Email = "test",
                PhoneNumber = "test"
            };
            accountServiceMock.Setup(a =>
                                     a.GetPersonalInformationAsync(It.IsAny<string>())).ReturnsAsync(userInfoDto);
            var claims = new ClaimsIdentity(new List<Claim>() { new Claim(ClaimTypes.Email, "test@mail.com") });
            var sut = new AccountController(accountServiceMock.Object, jwtServiceMock.Object);
            sut.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(claims)
            };
            //Act
            var result = await sut.GetPersonalInformationAsync();   
            //Assert
            Assert.IsType<OkObjectResult>(result);  
        }
        #endregion
        #region UpdateProfileAsync
        [Fact]
        public async Task UpdateProfileAsync_ValidData_ReturnsOk()
        {
            //Arrange
            var jwtServiceMock = new Mock<IJwtTokenService>();
            var accountServiceMock = new Mock<IAccountService>();
            var userUpdateDto = new UpdateUserDto()
            {
                UserName = "test",
                Email = "test",
                PhoneNumber = "234432"
            };
            accountServiceMock.Setup(a =>
                                     a.UpdateUserAsync(It.IsAny<string>(), It.IsAny<UpdateUserDto>())).ReturnsAsync(true);
            jwtServiceMock.Setup(j => j.GenerateJwtTokenAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync("token");
            var claims = new ClaimsIdentity(new List<Claim>() { 
                new Claim(ClaimTypes.Email, "test@mail.com"),
                new Claim(ClaimTypes.NameIdentifier, "2"),
            });
            var sut = new AccountController(accountServiceMock.Object, jwtServiceMock.Object);
            sut.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(claims)
            };
            //Act
            var result = await sut.UpdateProfileAsync(userUpdateDto);
            //Arrange
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<UserTokenResponse>((result as OkObjectResult).Value);
        }
        [Fact]
        public async Task UpdateProfileAsync_InvalidData_ReturnsBadRequest()
        {
            //Arrange
            var jwtServiceMock = new Mock<IJwtTokenService>();
            var accountServiceMock = new Mock<IAccountService>();
            var userUpdateDto = new UpdateUserDto()
            {
                UserName = "test",
                Email = "test",
                PhoneNumber = "test"
            };
            accountServiceMock.Setup(a =>
                                     a.UpdateUserAsync(It.IsAny<string>(), It.IsAny<UpdateUserDto>())).ReturnsAsync(false);
            var claims = new ClaimsIdentity(new List<Claim>() { new Claim(ClaimTypes.Email, "test@mail.com") });
            var sut = new AccountController(accountServiceMock.Object, jwtServiceMock.Object);
            sut.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(claims)
            };
            //Act
            var result = await sut.UpdateProfileAsync(userUpdateDto);
            //Arrange
            Assert.IsType<BadRequestObjectResult>(result);           
        }
        #endregion
    }
}
