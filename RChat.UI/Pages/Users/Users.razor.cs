using Microsoft.AspNetCore.Components;
using Radzen;
using RChat.Domain.Users;
using RChat.UI.Common.ComponentHelpers;
using RChat.UI.Services.UserService;
using RChat.UI.ViewModels;

namespace RChat.UI.Pages.Users
{
    public partial class UsersListComponent : ComponentBase, IListComponentBase<UserInformationViewModel>
    {
        [Inject]
        private IUserService UserService { get; set; }
        public string? SearchValue { get; set; }
        public int Count { get; set; }
        public int PageSize { get; set; } = 5;
        public IEnumerable<UserInformationViewModel> EntityList { get; set; } = new List<UserInformationViewModel>();

        protected override async Task OnInitializedAsync()
        {
            var apiResponse = await UserService.GetUsersListAsync(PageSize);
            if (apiResponse.IsSuccessStatusCode)
            {
                EntityList = apiResponse.Result.SelectedEntities!;
                Count = apiResponse.Result.TotalCount;
            }
        }
        public async Task OnChangeAsync()
        {
            var apiResponse = await UserService.GetUsersListAsync(PageSize, searchValue: SearchValue);
            EntityList = apiResponse.Result.SelectedEntities!;
            Count = apiResponse.Result.TotalCount;
        }

        protected async Task PageChanged(PagerEventArgs args)
        {
            var apiResult = await UserService.GetUsersListAsync(args.Top, args.Skip, SearchValue);
            EntityList = apiResult.Result.SelectedEntities;
        }

    }
}
