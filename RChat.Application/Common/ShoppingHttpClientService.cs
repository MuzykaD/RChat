using RChat.Application.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace RChat.Application.Common
{
    public class ShoppingHttpClientService : IShoppingHttpClientService
    {

        protected readonly HttpClient _httpClient;
        public ShoppingHttpClientService(IHttpClientFactory factory, IConfiguration config)
        {
            _httpClient = factory.CreateClient("ShoppingHttpClient");          
        }
        public HttpClient GetHttpClient()
            => _httpClient;       
    }
}
