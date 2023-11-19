using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using RChat.UI.Services.AccountService;
using RChat.UI.Services.SignalClientService;

namespace RChat.UI.Services.WebRtcService
{
    //Repo like style
    public class WebRtcService : IWebRtcService
    {
        private readonly NavigationManager _nav;
        private readonly IJSRuntime _js;
        private IJSObjectReference? _jsModule;
        private DotNetObjectReference<WebRtcService>? _jsThis;
        private ILocalStorageService _localStorageService;
        private HubConnection? _hub;
        private IAccountService _accountService;
        private string? _signalingChannel;
        public event EventHandler<IJSObjectReference>? OnRemoteStreamAcquired;
        private readonly string _hubHostUrl;

        public WebRtcService(IJSRuntime js,
            NavigationManager nav, 
            ILocalStorageService localStorageService,
            IAccountService accountService,
            IConfiguration configuration)
        {
            _js = js;
            _nav = nav;
            _localStorageService = localStorageService;
            _accountService = accountService;
            _hubHostUrl = configuration["ApiHost"]!;
        }

        public async Task StartAsync()
        {
            _hub = new HubConnectionBuilder()
                .WithUrl(_nav.ToAbsoluteUri($"{_hubHostUrl}/rVideoHub"), o => o.AccessTokenProvider =
                async () => await _localStorageService.GetItemAsync<string>("auth-jwt-token"))
                .Build();

            _hub.On<string, string, string>("SignalWebRtc", async (signalingChannel, type, payload) =>
            {
                if (_jsModule == null) throw new InvalidOperationException();

                if (_signalingChannel != signalingChannel) return;
                switch (type)
                {
                    case "offer":
                        await _jsModule.InvokeVoidAsync("processOffer", payload);
                        break;
                    case "answer":
                        await _jsModule.InvokeVoidAsync("processAnswer", payload);
                        break;
                    case "candidate":
                        await _jsModule.InvokeVoidAsync("processCandidate", payload);
                        break;
                }
            });
            await _hub.StartAsync();
        }

        public async Task Join(string signalingChannel)
        {
            if (_signalingChannel != null) throw new InvalidOperationException();
            _signalingChannel = signalingChannel;
            var hub = await GetHub();
            await hub.SendAsync("join", signalingChannel);
            _jsModule = await _js.InvokeAsync<IJSObjectReference>(
                "import", "/web-rtc-chat.js");
            _jsThis = DotNetObjectReference.Create(this);
            await _jsModule.InvokeVoidAsync("initialize", _jsThis);
        }
        public async Task<IJSObjectReference> StartLocalStream()
        {
            if (_jsModule == null) throw new InvalidOperationException();
            var stream = await _jsModule.InvokeAsync<IJSObjectReference>("startLocalStream");
            return stream;
        }
        public async Task Call()
        {
            if (_jsModule == null) throw new InvalidOperationException();
            var offerDescription = await _jsModule.InvokeAsync<string>("callAction");
            await SendOffer(offerDescription);
        }

        public async Task Hangup()
        {
            if (_jsModule == null) throw new InvalidOperationException();
            await _jsModule.InvokeVoidAsync("hangupAction");

            var hub = await GetHub();
            await hub.SendAsync("leave", _signalingChannel);

            _signalingChannel = null;
        }

        private async Task<HubConnection> GetHub()
        {
            if (_hub != null)
                return _hub;
            else
            { 
               await StartAsync();
                return _hub!;
            }          
        }

        [JSInvokable]
        public async Task SendOffer(string offer)
        {
            var hub = await GetHub();
            await hub.SendAsync("SignalWebRtc", _signalingChannel, "offer", offer);
        }

        [JSInvokable]
        public async Task SendAnswer(string answer)
        {
            var hub = await GetHub();
            await hub.SendAsync("SignalWebRtc", _signalingChannel, "answer", answer);
        }

        [JSInvokable]
        public async Task SendCandidate(string candidate)
        {
            var hub = await GetHub();
            await hub.SendAsync("SignalWebRtc", _signalingChannel, "candidate", candidate);
        }

        [JSInvokable]
        public async Task SetRemoteStream()
        {
            if (_jsModule == null) throw new InvalidOperationException();
            var stream = await _jsModule.InvokeAsync<IJSObjectReference>("getRemoteStream");
            OnRemoteStreamAcquired?.Invoke(this, stream);
        }

        public async Task RegisterUserSignalGroupsAsync()
        {
            var groupIds = await _accountService.GetUserSignalGroupsAsync();
            await _hub.SendAsync("RegisterMultipleGroupsAsync", groupIds.Result.SignalIdentifiers);
        }
    }
}
