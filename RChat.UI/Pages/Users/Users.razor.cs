using Microsoft.AspNetCore.Components;
using RChat.UI.Common.ComponentHelpers;
using RChat.UI.Services.UserService;
using RChat.UI.ViewModels;

namespace RChat.UI.Pages.Users
{
    public partial class UsersListComponent : ComponentBase, IListComponentBase<UserInformationViewModel>
    {
        [Inject]
        private IUserService UserService { get; set; }
        public IEnumerable<UserInformationViewModel> EntityList { get; set; } = new List<UserInformationViewModel>();

        protected override async Task OnInitializedAsync()
        {
            var apiResponse = await UserService.GetUsersListAsync();
            EntityList = apiResponse.Result!;
        }
    }
}
