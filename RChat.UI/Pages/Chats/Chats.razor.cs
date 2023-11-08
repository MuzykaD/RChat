using Microsoft.AspNetCore.Components;
using Radzen;
using RChat.UI.Common;
using RChat.UI.Common.ComponentHelpers;
using RChat.UI.Services.ChatService;
using RChat.UI.ViewModels;

namespace RChat.UI.Pages.Chats
{
    public partial class ChatsListComponent : ComponentBase, IListComponentBase<ChatInformationViewModel>, ISortComponent, ISearchComponent
    {
        [Inject]
        private IChatService ChatService { get; set; }
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
        private string _navigationQuery => "/chats" + HttpQueryBuilder.BuildGridListQuery(Page, Size, Value, OrderBy, OrderByType);

        public IEnumerable<ChatInformationViewModel> EntityList { get; set; } = new List<ChatInformationViewModel>();
        public IEnumerable<string> SortingFieldDropDown { get; set; }
        public IEnumerable<string> SortingTypeDropDown { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (Size == 0)
                Size = 5;
            await UpdateEntityList();
            SortingFieldDropDown = new List<string>() { "Id", "Name", "CreatorId" };

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
            var apiResponse = await ChatService.GetChatsListAsync(Page, Size, Value, OrderBy, OrderByType);
            if (apiResponse.IsSuccessStatusCode)
            {
                EntityList = apiResponse.Result.SelectedEntities!;
                Count = apiResponse.Result.TotalCount;
            }
        }
    }
}
