using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RChat.UI.Common.HttpClientPwa
{
    internal class HttpClientPwa
    {
        public const string LoginApiUrl = "https://localhost:7089/api/v1/authentication/login";
        public const string RegisterApiUrl = "https://localhost:7089/api/v1/authentication/register";


        public async Task<ApiRequestResult<TResult>> SendPostRequestAsync<TArgument, TResult>(string url, TArgument data)
        {
            using (var httpClient = new HttpClient())
            {
                var result = await httpClient.PostAsJsonAsync(url, data);
                return new ApiRequestResult<TResult>()
                {
                    IsSuccessStatusCode = result.IsSuccessStatusCode,
                    Result = await result.Content.ReadFromJsonAsync<TResult>(),
                    StatusCode = result.StatusCode,
                };
            }
        }
    }
}
