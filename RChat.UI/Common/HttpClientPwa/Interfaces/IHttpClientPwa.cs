namespace RChat.UI.Common.HttpClientPwa.Interfaces
{
    public interface IHttpClientPwa
    {
        public Task<ApiRequestResult<TResult>> SendPostRequestAsync<TArgument, TResult>(string url, TArgument data);
        public Task<ApiRequestResult<TResult>> SendPutRequestAsync<TArgument, TResult>(string url, TArgument data);
        public Task<ApiRequestResult<TResult>> SendGetRequestAsync< TResult>(string url);
        void TryAddJwtToken(string token);
        void TryDeleteJwtToken();


    }
}
