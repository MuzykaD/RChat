using HtmlAgilityPack;
using Microsoft.AspNetCore.Components;
using RChat.Application.Contracts.Assistant;
using RChat.Domain.Repsonses;
using RChat.Domain.Shopping;
using RChat.UI.Common;
using RChat.UI.Common.HttpClientPwa.Interfaces;
using RChat.UI.ViewModels.Shopping;
using System.Net;

namespace RChat.UI.Pages.Shopping
{
    public class ShoppingComponent : ComponentBase
    {
        [Inject]
        public IHttpClientPwa HttpClientPwa { get; set; }
        [Inject] 
        public IShoppingAssistantService ShoppingAssistantService { get; set; }
        public List<ShoppingProduct> EntityList { get; set; } = new();
        public string SearchValue { get; set; }

        protected async override Task OnInitializedAsync()
        {
            await ShoppingAssistantService.InitializeAsync("asst_7mGXlx3iUrxvFwlCh6TBpXK6");
        }

        protected async Task SearchProductsAsync()
        {
            if(!string.IsNullOrWhiteSpace(SearchValue))
            {
                var queryValue = SearchValue.Replace(' ', '+');
                var result = await HttpClientPwa.SendGetRequestAsync<GridListDto<ShoppingProduct>>(RChatApiRoutes.Shopping + $"?searchValue={queryValue}");
                EntityList = result.Result.SelectedEntities.ToList();
                StateHasChanged();
            }
               
           
        }

    }
}
