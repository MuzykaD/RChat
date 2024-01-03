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
        public List<ShoppingProduct> EntityList { get; set; } = new();
        public string SearchValue { get; set; }
        public int Amount { get; set; } = 3;
        public bool ProgressBarVisible { get; set; } = false;

        protected async override Task OnInitializedAsync()
        {
            
        }

        protected async Task SearchProductsAsync()
        {
            if(!string.IsNullOrWhiteSpace(SearchValue))
            {
                var queryValue = SearchValue.Replace(' ', '+');
                ProgressBarVisible = true;
                var result = await HttpClientPwa.SendGetRequestAsync<GridListDto<ShoppingProduct>>(RChatApiRoutes.Shopping + $"?searchValue={queryValue}&amount={Amount}");
                EntityList = result.Result.SelectedEntities.ToList();
                ProgressBarVisible = false;
                StateHasChanged();
            }
               
           
        }

    }
}
