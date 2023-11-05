using Blazored.LocalStorage;
using RChat.UI.Common.HttpClientPwa.Interfaces;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;


namespace RChat.UI.Common.HttpClientPwa
{
    internal class HttpClientPwa : IHttpClientPwa
    {

        public const string LoginApiUrl = "https://localhost:7089/api/v1/authentication/login";
        public const string RegisterApiUrl = "https://localhost:7089/api/v1/authentication/register";
        public const string TestApiUrl = "https://localhost:7089/api/v1/account/change-password";
        public const string Info = "https://localhost:7089/api/v1/account/profile";
        public const string UpdateInfo = "https://localhost:7089/api/v1/account/update-profile";
        public const string Users = "https://localhost:7089/api/v1/users";
        public const string SearchUsers = "https://localhost:7089/api/v1/users/search";
        public const string Qur = "https://localhost:7089/api/v1/users/qur";
        private ILocalStorageService LocalStorageService { get; set; }
        public HttpClientPwa(ILocalStorageService storageService)
        {
            LocalStorageService = storageService;
        }
        public async Task<ApiRequestResult<TResult>> SendPostRequestAsync<TArgument, TResult>(string url, TArgument data)
        {
            using (var httpClient = new HttpClient())
            {
                await TryAddJwtToken(httpClient);

                var apiResponse = await httpClient.PostAsJsonAsync(url, data);

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
        }
        public async Task<ApiRequestResult<TResult>> SendGetRequestAsync<TResult>(string url)
        {
            using (var httpClient = new HttpClient())
            {
                await TryAddJwtToken(httpClient);

                var apiResponse = await httpClient.GetAsync(url);

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
        }
        private async Task TryAddJwtToken(HttpClient client)
        {
            var token = await LocalStorageService.GetItemAsync<string>("auth-jwt-token");
            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

        }

      
    }
}
