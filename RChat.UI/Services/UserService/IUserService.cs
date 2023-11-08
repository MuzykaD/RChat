using RChat.Domain.Repsonses;
using RChat.Domain.Users;
using RChat.UI.Common;
using RChat.UI.ViewModels;

namespace RChat.UI.Services.UserService
{
    public interface IUserService
    {
        Task<ApiRequestResult<GridListDto<UserInformationViewModel>>> GetUsersListAsync( int page, int size, string? value = null, string? orderBy = null, string? orderByType = null);
    }
}
