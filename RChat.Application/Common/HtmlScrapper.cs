using HtmlAgilityPack;
using RChat.Application.Contracts.Common;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Application.Common
{
    //TODO Refactoring
    public class HtmlScrapper : IHtmlScrapper
    {
        public IShoppingHttpClientService ShoppingHttpClientService { get; set; }
        public HtmlScrapper(IShoppingHttpClientService shoppingHttpClientService)
        {

            ShoppingHttpClientService = shoppingHttpClientService;

        }
        public async Task<List<HtmlNode>> GetNodesWithAttributeByValueAsync(HtmlDocument html, string attribute, string value)
        {
            var result = html.DocumentNode
                .Descendants("div").Where(n => n.Attributes.Contains(attribute) && n.Attributes[attribute].Value.Equals(value)
                && n.Attributes[attribute] != null).ToList();
            return await Task.FromResult(result);
        }

        public async Task<string> GetPageAsStringByUrlAsync(string url)
        {
            /*
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    await using var stream = response.GetResponseStream();
                    using var reader = new StreamReader(stream);
                    var content = await reader.ReadToEndAsync();                
                    return content;
                }
                return null;
            }
            */

            var client = ShoppingHttpClientService.GetHttpClient();
            var response = await client.GetAsync(url);
            if (response.Content.Headers.ContentEncoding.Contains("gzip"))
            {
                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var gzipStream = new GZipStream(stream, CompressionMode.Decompress))
                using (var reader = new StreamReader(gzipStream, Encoding.UTF8))
                {
                   return reader.ReadToEnd();
                }
            }
            else
            {
                return await response.Content.ReadAsStringAsync();
            }

        }
    }
}
