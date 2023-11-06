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
        protected string SortFieldValue { get; set; }
        protected string SortTypeValue { get; set; }
        protected  bool IsSortingDisabled { get; set; }

        protected IEnumerable<string> SortingFieldDropDown { get; set; }
        protected IEnumerable<string> SortingTypeDropDown { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var apiResponse = await UserService.GetUsersListAsync(PageSize);
            if (apiResponse.IsSuccessStatusCode)
            {
                EntityList = apiResponse.Result.SelectedEntities!;
                Count = apiResponse.Result.TotalCount;

                SortingFieldDropDown = typeof(UserInformationViewModel).GetProperties().Select(p => p.Name);
                SortingTypeDropDown = new List<string>()
                {
                    "Ascending",
                    "Descending",
                };
            }
        }
        public async Task OnSearchChangeAsync()
        {
            var apiResponse = await UserService.GetUsersListAsync(PageSize, 
                                                                  searchValue: SearchValue, 
                                                                  orderBy: SortFieldValue, 
                                                                  orderByType: SortTypeValue);
            EntityList = apiResponse.Result.SelectedEntities!;
            Count = apiResponse.Result.TotalCount;
        }

        public async Task OnSortChangeAsync()
        {
            if(!IsSortingDisabled && !string.IsNullOrWhiteSpace(SortFieldValue) && !string.IsNullOrWhiteSpace(SortTypeValue))
            {
                var apiResponse = await UserService.GetUsersListAsync(PageSize,
                                                                  searchValue: SearchValue,
                                                                  orderBy: SortFieldValue,
                                                                  orderByType: SortTypeValue);
                EntityList = apiResponse.Result.SelectedEntities!;
                Count = apiResponse.Result.TotalCount;

            }
        }

        public void OnSwitch()
        {
            if(IsSortingDisabled)
            {
                SortFieldValue = null!;
                SortTypeValue = null!;
            }
        }

        protected async Task PageChanged(PagerEventArgs args)
        {
            var apiResult = await UserService.GetUsersListAsync(args.Top, args.Skip, SearchValue, SortFieldValue, SortTypeValue);
            EntityList = apiResult.Result.SelectedEntities;
        }        

    }
}
