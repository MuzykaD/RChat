using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
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
        [Inject] IJSRuntime Js { get; set; }
        protected IJSObjectReference? _module;
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
            await StartAction();
            await base.OnInitializedAsync();
            if(CallRequested)
            {
                await RtcService.ConfirmationResponse(_channel, CallRequested);
            }
        }     
        protected async Task AskForConfirmation()
        {
            await RtcService.AskForConfirmation(_channel, ChatId);
            _callDisabled = true;
            _hangupDisabled = false;
        }


        protected async Task StartAction()
        {
            if (string.IsNullOrWhiteSpace(_channel)) return;
            if (_module == null) throw new InvalidOperationException();
            var stream = await RtcService.StartLocalStream();
            await _module.InvokeVoidAsync("setLocalStream", stream);
            RtcService.OnRemoteStreamAcquired += RtcOnOnRemoteStreamAcquired;
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
            _callDisabled = false;
            _hangupDisabled = true;
            await _module.InvokeVoidAsync("setRemoteStreamToNull");
        }


    }
}
