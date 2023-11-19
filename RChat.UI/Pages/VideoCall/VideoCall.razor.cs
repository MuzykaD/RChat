using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using RChat.UI.Services.SignalVideoService;
using RChat.UI.Services.WebRtcService;
using System.Reflection;
using System.Security.Claims;

namespace RChat.UI.Pages.VideoCall
{
    public class VideoCallComponent : ComponentBase
    {
        [Inject] IWebRtcService RtcService { get; set; }
        [Inject] IJSRuntime Js { get; set; }
        protected IJSObjectReference? _module;
        protected bool _startDisabled;
        protected bool _callDisabled = true;
        protected bool _hangupDisabled = true;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _module = await Js.InvokeAsync<IJSObjectReference>(
                    "import", "./Pages/VideoCall/VideoCall.razor.js");
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        protected async Task StartAction()
        {
            if (string.IsNullOrWhiteSpace(_channel)) return;
            if (_module == null) throw new InvalidOperationException();
            if (_startDisabled) return;
            _startDisabled = true;
            await RtcService.Join(_channel);
            var stream = await RtcService.StartLocalStream();
            await _module.InvokeVoidAsync("setLocalStream", stream);
            RtcService.OnRemoteStreamAcquired += RtcOnOnRemoteStreamAcquired;
            _callDisabled = false;
        }

        protected async void RtcOnOnRemoteStreamAcquired(object? _, IJSObjectReference e)
        {
            if (_module == null) throw new InvalidOperationException();
            await _module.InvokeVoidAsync("setRemoteStream", e);
            _callDisabled = true;
            _hangupDisabled = false;
            _startDisabled = true;
            StateHasChanged();
        }

        protected async Task CallAction()
        {
            if (_callDisabled) return;
            _callDisabled = true;
            await RtcService.Call();
            _hangupDisabled = false;
        }
        protected async Task HangupAction()
        {
            await RtcService.Hangup();
            _callDisabled = true;
            _hangupDisabled = true;
            _startDisabled = false;
        }

        protected string _channel = "foo";
    }
}
