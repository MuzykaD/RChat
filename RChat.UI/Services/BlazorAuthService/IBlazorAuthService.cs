using RChat.Domain.Repsonses;
using RChat.Domain.Users.DTO;
using RChat.UI.Common;
using RChat.UI.Common.HttpClientPwa.Interfaces;
using RChat.UI.ViewModels;

namespace RChat.UI.Services.BlazorAuthService
{
    public interface IBlazorAuthService
    {
        public Task<ApiRequestResult<ApiResponse>> RegisterUserAsync(RegisterViewModel registerViewModel);
        public Task<ApiRequestResult<UserTokenResponse>> LoginUserAsync(LoginViewModel registerViewModel);
        public Task LogoutUserAsync();
      
    }
}
