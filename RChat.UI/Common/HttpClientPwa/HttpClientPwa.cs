using Blazored.LocalStorage;
using RChat.UI.Common.HttpClientPwa.Interfaces;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RChat.UI.Common.HttpClientPwa
{
    internal class HttpClientPwa : IHttpClientPwa
    {

        public const string LoginApiUrl = "https://localhost:7089/api/v1/authentication/login";
        public const string RegisterApiUrl = "https://localhost:7089/api/v1/authentication/register";
        public const string TestApiUrl = "https://localhost:7089/api/v1/authentication/access-point";
        private ILocalStorageService LocalStorageService { get; set; }
        private bool _jwtRequired;
        public HttpClientPwa(ILocalStorageService storageService)
        {
            LocalStorageService = storageService;
        }
        public async Task<ApiRequestResult<TResult>> SendPostRequestAsync<TArgument, TResult>(string url, TArgument data)
        {
            using (var httpClient = new HttpClient())
            {

                if (_jwtRequired)
                    httpClient.DefaultRequestHeaders.Authorization = 
                        new AuthenticationHeaderValue("Bearer", await LocalStorageService.GetItemAsync<string>("auth-jwt-token"));
                var apiResponse = await httpClient.PostAsJsonAsync(url, data);

                _jwtRequired = false;

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

        public IHttpClientPwa UsingJwtToken()
        {
            _jwtRequired = true;
            return this;
        }

    }
}
