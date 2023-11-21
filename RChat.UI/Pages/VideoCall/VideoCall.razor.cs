using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using RChat.UI.Services.WebRtcService;
using System.Reflection;
using System.Security.Claims;

namespace RChat.UI.Pages.VideoCall
{
    public class VideoCallComponent : ComponentBase
    {
        [CascadingParameter]
        IWebRtcService RtcService { get; set; }
        [Inject]
        IJSRuntime Js { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        protected IJSObjectReference? _module;
        protected IJSObjectReference? _stream;
        protected bool _callDisabled = false;
        protected bool _hangupDisabled = true;
        [Parameter]
        [SupplyParameterFromQuery]
        public int ChatId { get; set; }
        [Parameter]
        [SupplyParameterFromQuery(Name = "requestCall")]
        public string? RequestCallValue { get; set; }
        protected bool CallRequested => !string.IsNullOrWhiteSpace(RequestCallValue) && RequestCallValue.Equals("true", StringComparison.OrdinalIgnoreCase);
        protected string _channel => $"video-{ChatId}";
        protected override async Task OnInitializedAsync()
        {
            _module = await Js.InvokeAsync<IJSObjectReference>(
                   "import", "./Pages/VideoCall/VideoCall.razor.js");
            await RtcService.Join(_channel);
            NavigationManager.LocationChanged += LocationChanged;
            await StartAction();
            await base.OnInitializedAsync();
            if (CallRequested)
            {
                await RtcService.ConfirmationResponse(_channel, CallRequested);
            }
        }
        protected async Task AskForConfirmation()
        {
            await RtcService.AskForConfirmation(_channel, ChatId);

        }


        protected async Task StartAction()
        {
            if (string.IsNullOrWhiteSpace(_channel)) return;
            if (_module == null) throw new InvalidOperationException();
            var stream = await RtcService.StartLocalStream();
            _stream = stream;
            await _module.InvokeVoidAsync("setLocalStream", stream);
            RtcService.OnRemoteStreamAcquired += RtcOnOnRemoteStreamAcquired;
            RtcService.OnCallAccepted += OnCallAccepted;
            await Console.Out.WriteLineAsync("Video added");
        }

        protected async void RtcOnOnRemoteStreamAcquired(object? _, IJSObjectReference e)
        {
            if (_module == null) throw new InvalidOperationException();
            await _module.InvokeVoidAsync("setRemoteStream", e);
            _callDisabled = true;
            _hangupDisabled = false;
            StateHasChanged();
        }
        protected async Task HangupAction()
        {
            await RtcService.Hangup();
            _callDisabled = false;
            _hangupDisabled = true;
            await _module.InvokeVoidAsync("setRemoteStreamToNull");
        }

        protected void OnCallAccepted()
        {
            _callDisabled = true;
            _hangupDisabled = false;
        }

        async void LocationChanged(object sender, LocationChangedEventArgs e)
        {
            if (_callDisabled)
                await RtcService.Hangup();
            NavigationManager.LocationChanged -= LocationChanged;
            await _module.InvokeVoidAsync("stopCameraAndMic", _stream);
        }       
    }
}
