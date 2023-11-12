using Microsoft.AspNetCore.Components;
using Radzen;
using RChat.Domain.Users;
using RChat.UI.Common;
using RChat.UI.Common.ComponentHelpers;
using RChat.UI.Services.UserService;
using RChat.UI.ViewModels.InformationViewModels;

namespace RChat.UI.Pages.Users
{
    public partial class UsersListComponent : ComponentBase, IListComponentBase<UserInformationViewModel>, ISortComponent, ISearchComponent
    {
        [Inject]
        private IUserService UserService { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        [SupplyParameterFromQuery]
        [Parameter]
        public string? Value { get; set; }

        [SupplyParameterFromQuery]
        [Parameter]
        public int Size { get; set; }
        [SupplyParameterFromQuery]
        [Parameter]
        public string OrderBy { get; set; }
        [SupplyParameterFromQuery]
        [Parameter]
        public string OrderByType { get; set; }

        [SupplyParameterFromQuery]
        [Parameter]
        public int Page { get; set; }
        public bool IsSortingDisabled { get; set; } = true;

        public int Count { get; set; }
        private string _navigationQuery => "/users" + HttpQueryBuilder.BuildGridListQuery(Page, Size, Value, OrderBy, OrderByType);

        public IEnumerable<UserInformationViewModel> EntityList { get; set; } = new List<UserInformationViewModel>();
        public IEnumerable<string> SortingFieldDropDown { get; set; }
        public IEnumerable<string> SortingTypeDropDown { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (Size == 0)
                Size = 5;
            await UpdateEntityList();
            SortingFieldDropDown = typeof(UserInformationViewModel).GetProperties().Select(p => p.Name);
            SortingTypeDropDown = new List<string>()
                {
                    "Ascending",
                    "Descending",
                };
        }
        public async Task OnSearchChangeAsync()
        {
            await UpdateEntityList();
            NavigationManager.NavigateTo(_navigationQuery);
        }

        public async Task OnSortChangeAsync()
        {
            if (!IsSortingDisabled && !string.IsNullOrWhiteSpace(OrderBy) && !string.IsNullOrWhiteSpace(OrderByType))
            {
                await UpdateEntityList();
                NavigationManager.NavigateTo(_navigationQuery);
            }
        }

        public async Task OnSortSwitchAsync()
        {
            if (IsSortingDisabled)
            {
                OrderBy = null!;
                OrderByType = null!;
                await UpdateEntityList();
                NavigationManager.NavigateTo(_navigationQuery);
            }
        }

        public async Task OnPageChangedAsync(PagerEventArgs args)
        {
            Page = args.PageIndex;
            await UpdateEntityList();
            NavigationManager.NavigateTo(_navigationQuery);
        }

        private async Task UpdateEntityList()
        {
            var apiResponse = await UserService.GetInformationListAsync(Page, Size, Value, OrderBy, OrderByType);
            if (apiResponse.IsSuccessStatusCode)
            {
                EntityList = apiResponse.Result.SelectedEntities!;
                Count = apiResponse.Result.TotalCount;
            }
        }

        protected void StartChat(string email)
        {
            NavigationManager.NavigateTo($"/chats/private?email={email}");
        }
    }
}
