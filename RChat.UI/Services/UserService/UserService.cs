using RChat.Domain.Repsonses;
using RChat.Domain.Users.DTO;
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

        public async Task<ApiRequestResult<PersonalPageViewModel>> GetPersonalInformationAsync()
        {
            return await _httpClientPwa.SendGetRequestAsync<PersonalPageViewModel>(HttpClientPwa.Info);
        }

        public async Task<ApiRequestResult<UserTokenResponse>> UpdatePersonalInformationAsync(PersonalPageViewModel personalPageViewModel)
        {
            return await _httpClientPwa.SendPostRequestAsync<PersonalPageViewModel, UserTokenResponse>(HttpClientPwa.UpdateInfo, personalPageViewModel);
        }
    }
}
