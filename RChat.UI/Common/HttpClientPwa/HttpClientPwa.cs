using Blazored.LocalStorage;
using RChat.UI.Common.HttpClientPwa.Interfaces;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;


namespace RChat.UI.Common.HttpClientPwa
{
    internal class HttpClientPwa : IHttpClientPwa
    {
        private ILocalStorageService LocalStorageService { get; set; }
        private HttpClient HttpClient { get; set; }
        public HttpClientPwa(ILocalStorageService storageService, HttpClient httpClient)
        {
            LocalStorageService = storageService;
            HttpClient = httpClient;
        }
        public async Task<ApiRequestResult<TResult>> SendPostRequestAsync<TArgument, TResult>(string url, TArgument data)
        {           

            var apiResponse = await HttpClient.PostAsJsonAsync(url, data);
            return (apiResponse.StatusCode.Equals(HttpStatusCode.Unauthorized)
               && !apiResponse.IsSuccessStatusCode) ?
               new ApiRequestResult<TResult>()
               {
                   IsSuccessStatusCode = apiResponse.IsSuccessStatusCode,
                   Result = default,
                   StatusCode = apiResponse.StatusCode,
                   Message = "Please, log in to the RChat to continue!"
               } :
               new ApiRequestResult<TResult>()
               {
                   IsSuccessStatusCode = apiResponse.IsSuccessStatusCode,
                   Result = await apiResponse.Content.ReadFromJsonAsync<TResult>(),
                   StatusCode = apiResponse.StatusCode,
               };
        }

        public async Task<ApiRequestResult<TResult>> SendPutRequestAsync<TArgument, TResult>(string url, TArgument data)
        {
            var apiResponse = await HttpClient.PutAsJsonAsync(url, data);
            return (apiResponse.StatusCode.Equals(HttpStatusCode.Unauthorized)
               && !apiResponse.IsSuccessStatusCode) ?
               new ApiRequestResult<TResult>()
               {
                   IsSuccessStatusCode = apiResponse.IsSuccessStatusCode,
                   Result = default,
                   StatusCode = apiResponse.StatusCode,
                   Message = "Please, log in to the RChat to continue!"
               } :
               new ApiRequestResult<TResult>()
               {
                   IsSuccessStatusCode = apiResponse.IsSuccessStatusCode,
                   Result = await apiResponse.Content.ReadFromJsonAsync<TResult>(),
                   StatusCode = apiResponse.StatusCode,
               };
        }

        public async Task<ApiRequestResult<TResult>> SendGetRequestAsync<TResult>(string url)
        {
            var apiResponse = await HttpClient.GetAsync(url);

            return (apiResponse.StatusCode.Equals(HttpStatusCode.Unauthorized)
               && !apiResponse.IsSuccessStatusCode) ?
               new ApiRequestResult<TResult>()
               {
                   IsSuccessStatusCode = apiResponse.IsSuccessStatusCode,
                   Result = default,
                   StatusCode = apiResponse.StatusCode,
                   Message = "Please, log in to the RChat to continue!"
               } :
               new ApiRequestResult<TResult>()
               {
                   IsSuccessStatusCode = apiResponse.IsSuccessStatusCode,
                   Result = await apiResponse.Content.ReadFromJsonAsync<TResult>(),
                   StatusCode = apiResponse.StatusCode,
               };

        }
        public  void TryAddJwtToken(string token)
        {
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }



        public void TryDeleteJwtToken()
        {
            HttpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task SendDeleteRequestAsync(string url)
        {
            await HttpClient.DeleteAsync(url);
        }
    }
}
