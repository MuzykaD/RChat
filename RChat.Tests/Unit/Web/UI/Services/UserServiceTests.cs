using Moq;
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
using RChat.UI.Services.UserService;

namespace RChat.Tests.Unit.Web.UI.Services
{
    public class UserServiceTests
    {

        [Fact]
        public async Task GetInformationListAsync_Should_Send_GetRequest_To_Correct_Endpoint_With_Query_Parameters()
        {
            // Arrange
            var httpClientMock = new Mock<IHttpClientPwa>();
            var userService = new UserService(httpClientMock.Object);

            var page = 1;
            var size = 10;
            string? value = "searchValue";
            string? orderBy = "UserName";
            string? orderByType = "Ascending";
           

            httpClientMock.Setup(client => client.SendGetRequestAsync<GridListDto<UserInformationViewModel>>(It.IsAny<string>()))
                .ReturnsAsync(new ApiRequestResult<GridListDto<UserInformationViewModel>> { Result = new(), IsSuccessStatusCode = true });

            // Act
            var result = await userService.GetInformationListAsync(page, size, value, orderBy, orderByType);

            // Assert
            Assert.IsType<ApiRequestResult<GridListDto<UserInformationViewModel>>>(result);
            Assert.True(result.IsSuccessStatusCode);
            Assert.IsType<GridListDto<UserInformationViewModel>> (result.Result);
            httpClientMock.Verify(
                client => client.SendGetRequestAsync<GridListDto<UserInformationViewModel>>(It.IsAny<string>()),
                Times.Once);
        }
    }
}
