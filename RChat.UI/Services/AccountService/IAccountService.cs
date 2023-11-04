using RChat.Domain.Repsonses;
using RChat.Domain.Users.DTO;
using RChat.UI.Common;
using RChat.UI.ViewModels;

namespace RChat.UI.Services.AccountService
{
    public interface IAccountService
    {
        public Task<ApiRequestResult<ApiResponse>> ChangeUserPasswordAsync(ChangePasswordViewModel changePasswordModel);
        public Task<ApiRequestResult<UserInformationViewModel>> GetPersonalInformationAsync();
        public Task<ApiRequestResult<UserTokenResponse>> UpdatePersonalInformationAsync(UserInformationViewModel personalPageViewModel);


    }
}
