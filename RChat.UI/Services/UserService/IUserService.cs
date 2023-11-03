using RChat.Domain.Repsonses;
using RChat.UI.Common;
using RChat.UI.ViewModels;

namespace RChat.UI.Services.UserService
{
    public interface IUserService
    {
        public Task<ApiRequestResult<ApiResponse>> ChangeUserPasswordAsync(ChangePasswordViewModel changePasswordModel);
    }
}
