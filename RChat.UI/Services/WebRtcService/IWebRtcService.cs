using Microsoft.JSInterop;

namespace RChat.UI.Services.WebRtcService
{
    public interface IWebRtcService
    {
        event EventHandler<IJSObjectReference>? OnRemoteStreamAcquired;
        event Action OnCallAccepted;
        Task Join(string signalingChannel);
        Task<IJSObjectReference> StartLocalStream();
        Task Call();
        Task Hangup();
        Task SendOffer(string offer);
        Task StartAsync();
        Task RegisterUserSignalGroupsAsync();
        Task AskForConfirmation(string channel, int chatId);
        Task ConfirmationResponse(string channel, bool result);
    }
}
