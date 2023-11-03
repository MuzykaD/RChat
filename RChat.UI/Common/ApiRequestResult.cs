using System.Net;

namespace RChat.UI.Common
{
    public class ApiRequestResult<TResult>
    {
        public TResult? Result { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccessStatusCode { get; set; }
        public string? Message { get; set; }
    }
}
