using Microsoft.AspNetCore.Components;
using Radzen;
using RChat.UI.Common;
using RChat.UI.Common.ComponentHelpers;
using RChat.UI.Services.MessageService;
using RChat.UI.ViewModels.InformationViewModels;

namespace RChat.UI.Pages.Messages
{
    public partial class MessagesListComponent : ComponentBase, IListComponentBase<MessageInformationViewModel>, ISortComponent, ISearchComponent
    {
        [Inject]
        private IMessageService MessageService { get; set; }
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
        private string _navigationQuery => "/messages" + HttpQueryBuilder.BuildGridListQuery(Page, Size, Value, OrderBy, OrderByType);

        public IEnumerable<MessageInformationViewModel> EntityList { get; set; } = new List<MessageInformationViewModel>();
        public IEnumerable<string> SortingFieldDropDown { get; set; }
        public IEnumerable<string> SortingTypeDropDown { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (Size == 0)
                Size = 5;
            await UpdateEntityList();

            SortingFieldDropDown = typeof(MessageInformationViewModel)
                .GetProperties()
                .Select(p => p.Name)
                .ToList();

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
            var apiResponse = await MessageService.GetInformationListAsync(Page, Size, Value, OrderBy, OrderByType);
            if (apiResponse.IsSuccessStatusCode)
            {
                EntityList = apiResponse.Result.SelectedEntities!;
                Count = apiResponse.Result.TotalCount;
            }
        }
    }
}
