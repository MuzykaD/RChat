using RChat.UI.Common;
using RChat.UI.ViewModels;

namespace RChat.UI.Services.UserService
{
    public interface IUserService
    {
        Task<ApiRequestResult<IEnumerable<UserInformationViewModel>>> GetUsersListAsync(); 
    }
}
