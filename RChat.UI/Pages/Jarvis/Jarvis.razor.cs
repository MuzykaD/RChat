using Microsoft.AspNetCore.Components;
using RChat.UI.Services.JarvisService;

namespace RChat.UI.Pages.Jarvis
{
    public class JarvisComponent : ComponentBase
    {
        public List<string> Messages { get; set; } = new();
        public string? MessageToSend { get; set; }
        [Inject]
        public IJarvisService JarvisService { get; set; }


        protected async Task SendMessageAsync()
        {
            if (!string.IsNullOrWhiteSpace(MessageToSend))
            {               
                var jarvisResponse = await JarvisService.SendMessageToJarvisAsync(MessageToSend!);
                Messages.Add($"Me: {MessageToSend}");
                Messages.Add($"AI: {jarvisResponse}");
                Messages = new List<string>(Messages);
                MessageToSend = string.Empty;
                StateHasChanged();
            }
        }
    }
}
