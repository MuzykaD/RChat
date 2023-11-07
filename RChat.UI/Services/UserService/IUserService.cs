using RChat.Domain.Repsonses;
using RChat.Domain.Users;
using RChat.UI.Common;
using RChat.UI.ViewModels;

namespace RChat.UI.Services.UserService
{
    public interface IUserService
    {
        Task<ApiRequestResult<GridListDto<UserInformationViewModel>>> GetUsersListAsync(int take, int skip = 0, string? searchValue = null, string? orderBy = null, string? orderByType = null);
    }
}
