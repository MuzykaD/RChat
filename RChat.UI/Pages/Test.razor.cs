using Microsoft.AspNetCore.Components;
using RChat.Domain.Repsonses;
using RChat.UI.Common.HttpClientPwa;
using RChat.UI.Common.HttpClientPwa.Interfaces;
using RChat.UI.ViewModels;

namespace RChat.UI.Pages
{
    public partial class TestComponent : ComponentBase
    {

        public string Message { get; set; }
        public bool ResponseReceived { get; set; }
        [Inject]
        public IHttpClientPwa HttpClient { get; set; }

        public async Task TestPoint()
        {
            var response = await HttpClient.
                SendPostRequestAsync<LoginViewModel, ApiResponse>(HttpClientPwa.TestApiUrl, new LoginViewModel() { Email = "asd@asd.net", Password = "123" });

            Message = response.IsSuccessStatusCode ? response.Result.Message : response.Message;
            ResponseReceived = true;
        }
    }
}
