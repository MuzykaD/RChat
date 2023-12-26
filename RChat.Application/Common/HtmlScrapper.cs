using HtmlAgilityPack;
using RChat.Application.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Application.Common
{
    //TODO Refactoring
    public class HtmlScrapper : IHtmlScrapper
    {
        public async Task<string> GetPageAsStringByUrlAsync(string url)
        {
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
        }
    }
}
