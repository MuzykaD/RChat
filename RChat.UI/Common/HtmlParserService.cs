using HtmlAgilityPack;
using System.Xml;

namespace RChat.UI.Common
{
    public static class HtmlParserService
    {
        public static List<HtmlNode> GetNodesWithCssClass(string html, string cssClass)
        {
            // Load the HTML document
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            // Select nodes with the specified CSS class
            var nodes = doc.DocumentNode.Descendants().Where(n =>
                n.Attributes["class"] != null &&
                n.Attributes["class"].Value.Contains(cssClass, StringComparison.InvariantCultureIgnoreCase)
            ).ToList();

            return nodes;
        }
    }
}
