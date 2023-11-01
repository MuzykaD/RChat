namespace RChat.UI.Common.HttpClientPwa.Interfaces
{
    public interface IHttpClientPwa
    {
        public Task<ApiRequestResult<TResult>> SendPostRequestAsync<TArgument, TResult>(string url, TArgument data, bool addJwt = true);
       
    }
}
