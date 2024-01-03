using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Application.Contracts.Common
{
    public interface IHtmlScrapper
    {
        public Task<string> GetPageAsStringByUrlAsync(string url);
        public Task<List<HtmlNode>> GetNodesWithAttributeByValueAsync(HtmlDocument html,string attribute, string value);
    }
}
