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
        protected string ShopUrl { get; set; }
        public ShoppingController(IShoppingAssistantService shoppingAssistantService,
                                  IHtmlScrapper htmlScrapper,
                                  IConfiguration config)
        {
            ShoppingAssistantService = shoppingAssistantService;
            HtmlScrapper = htmlScrapper;
            ShopUrl = config["ShopUrl"]!;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsFromPage([FromQuery] string searchValue)
        {
            await ShoppingAssistantService.InitializeAsync("asst_7mGXlx3iUrxvFwlCh6TBpXK6");
            var fullUrl = ShopUrl + searchValue.Replace(' ', '+');
            var htmlPage = await HtmlScrapper.GetPageAsStringByUrlAsync(fullUrl);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlPage);
            var nodes = htmlDocument.DocumentNode
                .Descendants("div").Where(n => n.Attributes.Contains("data-component-type") && n.Attributes["data-component-type"].Value.Equals("s-search-result")
                && n.Attributes["data-component-type"] != null).ToList();
            var result = await ShoppingAssistantService.GetListOfShoppingProductsAsync(nodes.Select(n => n.OuterHtml).ToArray());

            return Ok(new GridListDto<ShoppingProduct>()
            {
                SelectedEntities = result,
                TotalCount = result.Count()
            });
        }
    }
}
