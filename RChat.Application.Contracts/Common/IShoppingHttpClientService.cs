using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Application.Contracts.Common
{
    public interface IShoppingHttpClientService
    {
        public HttpClient GetHttpClient();
    }
}
