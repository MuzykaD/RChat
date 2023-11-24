using Moq;
using RChat.Domain.Repsonses;
using RChat.Domain.Users.DTO;
using RChat.UI.Common;
using RChat.UI.Common.HttpClientPwa.Interfaces;
using RChat.UI.Services.AccountService;
using RChat.UI.ViewModels.InformationViewModels;
using RChat.UI.ViewModels.ProfileViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RChat.Tests.Unit.Web.UI.Services
{
    public class AccountServiceTests
    {
        [Fact]
        public async Task ChangeUserPasswordAsync_Should_Return_Result_From_HttpClient()
        {
            // Arrange
            var httpClientMock = new Mock<IHttpClientPwa>();
            var accountService = new AccountService(httpClientMock.Object);
            var changePasswordModel = new ChangePasswordViewModel();
            var expectedResult = new ApiRequestResult<ApiResponse> { IsSuccessStatusCode = true };
            httpClientMock.Setup(client => client.SendPutRequestAsync<ChangePasswordViewModel, ApiResponse>(
                    RChatApiRoutes.ChangePassword, changePasswordModel))
                .ReturnsAsync(expectedResult);
            // Act
            var result = await accountService.ChangeUserPasswordAsync(changePasswordModel);

            // Assert
            Assert.Same(expectedResult, result);
        }
        [Fact]
        public async Task GetPersonalInformationAsync_Should_Return_Result_From_HttpClient()
        {
            // Arrange
            var httpClientMock = new Mock<IHttpClientPwa>();
            var accountService = new AccountService(httpClientMock.Object);

            var expectedResult = new ApiRequestResult<UserInformationViewModel> { IsSuccessStatusCode = true };

            httpClientMock.Setup(client => client.SendGetRequestAsync<UserInformationViewModel>(RChatApiRoutes.Info))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await accountService.GetPersonalInformationAsync();

            // Assert
            Assert.Same(expectedResult, result);
        }
        [Fact]
        public async Task GetUserSignalGroupsAsync_Should_Return_Result_From_HttpClient()
        {
            // Arrange
            var httpClientMock = new Mock<IHttpClientPwa>();
            var accountService = new AccountService(httpClientMock.Object);

            var expectedResult = new ApiRequestResult<GroupsIdentifies> { IsSuccessStatusCode = true };

            httpClientMock.Setup(client => client.SendGetRequestAsync<GroupsIdentifies>(RChatApiRoutes.SignalGroupsInfo))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await accountService.GetUserSignalGroupsAsync();

            // Assert
            Assert.Same(expectedResult, result);
        }
        [Fact]
        public async Task UpdatePersonalInformationAsync_Should_Return_Result_From_HttpClient_And_Update_JwtToken()
        {
            // Arrange
            var httpClientMock = new Mock<IHttpClientPwa>();
            var accountService = new AccountService(httpClientMock.Object);

            var personalPageViewModel = new UserInformationViewModel();
            var expectedResult = new ApiRequestResult<UserTokenResponse>
            {
                Result = new() { Token = "token" },
                IsSuccessStatusCode = true
            };

            httpClientMock.Setup(client => client.SendPutRequestAsync<UserInformationViewModel, UserTokenResponse>(
                    RChatApiRoutes.UpdateInfo, personalPageViewModel))
                .ReturnsAsync(expectedResult);
            // Act
            var result = await accountService.UpdatePersonalInformationAsync(personalPageViewModel);
            // Assert
            Assert.Same(expectedResult, result);
            httpClientMock.Verify(client => client.TryAddJwtToken(expectedResult.Result.Token), Times.Once);
        }
    }
}
