using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RChat.Application.Contracts.Assistant;
using RChat.Application.Contracts.Common;
using RChat.Domain.Repsonses;
using RChat.Domain.Shopping;

namespace RChat.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/shopping")]
    public class ShoppingController : ControllerBase
    {
        protected IShoppingAssistantService ShoppingAssistantService { get; set; }
        protected IHtmlScrapper HtmlScrapper { get; set; }
        private string _shopUrl;
        private string _shoppingAssistantId;
        public ShoppingController(IShoppingAssistantService shoppingAssistantService,
                                  IHtmlScrapper htmlScrapper,
                                  IConfiguration config)
        {
            ShoppingAssistantService = shoppingAssistantService;
            HtmlScrapper = htmlScrapper;
            _shopUrl = config["ShopUrl"]!;
            _shoppingAssistantId = config["ShoppingAssistantId"]!;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsFromPage([FromQuery] string searchValue, [FromQuery] int amount)
        {
            await ShoppingAssistantService.InitializeAsync("asst_7mGXlx3iUrxvFwlCh6TBpXK6");
           // var fullUrl = _shopUrl + searchValue.Replace(' ', '+');
            var htmlPage = await HtmlScrapper.GetPageAsStringByUrlAsync("s?k=" + searchValue.Replace(' ', '+'));
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlPage);
            var nodes = await HtmlScrapper.GetNodesWithAttributeByValueAsync(htmlDocument, "data-component-type", "s-search-result");
            var result = await ShoppingAssistantService.GetListOfShoppingProductsAsync(nodes.Select(n => n.OuterHtml).Take(amount).ToArray());

            return Ok(new GridListDto<ShoppingProduct>()
            {
                SelectedEntities = result,
                TotalCount = result.Count()
            });
        }
    }
}
