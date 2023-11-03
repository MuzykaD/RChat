using RChat.Domain.Repsonses;
using RChat.UI.Common;
using RChat.UI.Common.HttpClientPwa;
using RChat.UI.Common.HttpClientPwa.Interfaces;
using RChat.UI.ViewModels;

namespace RChat.UI.Services.UserService
{
    public class UserService : IUserService
    {
        private IHttpClientPwa _httpClientPwa;
        public UserService(IHttpClientPwa httpClientPwa)
        {
            _httpClientPwa = httpClientPwa;
        }
        public async Task<ApiRequestResult<ApiResponse>> ChangeUserPasswordAsync(ChangePasswordViewModel changePasswordModel)
        {
            return await _httpClientPwa.SendPostRequestAsync<ChangePasswordViewModel, ApiResponse>(HttpClientPwa.TestApiUrl, changePasswordModel);
        }
    }
}
