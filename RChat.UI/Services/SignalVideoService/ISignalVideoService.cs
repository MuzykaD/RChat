using Microsoft.AspNetCore.SignalR.Client;

namespace RChat.UI.Services.SignalVideoService
{
    public interface ISignalVideoService
    {
        public event Action OnCallTerminated;
        public event Action OnCallDeclined;
        public Task StartAsync();
        public HubConnection GetHub();
    }
}
