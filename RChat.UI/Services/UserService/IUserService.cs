using RChat.Domain.Repsonses;
using RChat.Domain.Users;
using RChat.UI.Common;
using RChat.UI.Services.Common;
using RChat.UI.ViewModels.InformationViewModels;

namespace RChat.UI.Services.UserService
{
    public interface IUserService : IModelInformationList<UserInformationViewModel>
    {
    }
}
