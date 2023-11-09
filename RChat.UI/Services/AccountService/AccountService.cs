using RChat.Domain.Repsonses;
using RChat.Domain.Users.DTO;
using RChat.UI.Common;
using RChat.UI.Common.HttpClientPwa;
using RChat.UI.Common.HttpClientPwa.Interfaces;
using RChat.UI.ViewModels;

namespace RChat.UI.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private IHttpClientPwa _httpClientPwa;
        public AccountService(IHttpClientPwa httpClientPwa)
        {
            _httpClientPwa = httpClientPwa;
        }
        public async Task<ApiRequestResult<ApiResponse>> ChangeUserPasswordAsync(ChangePasswordViewModel changePasswordModel)
        {
            return await _httpClientPwa.SendPutRequestAsync<ChangePasswordViewModel, ApiResponse>(RChatApiRoutes.ChangePassword, changePasswordModel);
        }

        public async Task<ApiRequestResult<UserInformationViewModel>> GetPersonalInformationAsync()
        {
            return await _httpClientPwa.SendGetRequestAsync<UserInformationViewModel>(RChatApiRoutes.Info);
        }

        public async Task<ApiRequestResult<UserTokenResponse>> UpdatePersonalInformationAsync(UserInformationViewModel personalPageViewModel)
        {
            var result = await _httpClientPwa.SendPutRequestAsync<UserInformationViewModel, UserTokenResponse>(RChatApiRoutes.UpdateInfo, personalPageViewModel);
            if (result.IsSuccessStatusCode)
                _httpClientPwa.TryAddJwtToken(result.Result.Token);
            return result;

        }
    }
}
