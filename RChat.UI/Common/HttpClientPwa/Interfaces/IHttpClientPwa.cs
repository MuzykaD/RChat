namespace RChat.UI.Common.HttpClientPwa.Interfaces
{
    public interface IHttpClientPwa
    {
        public IHttpClientPwa WithJwt();
        public Task<ApiRequestResult<TResult>> SendPostRequestAsync<TArgument, TResult>(string url, TArgument data);
       
    }
}
