
using Microsoft.SemanticKernel;

namespace RChat.Application.Contracts.ChatKernel
{
    public interface IChatKernelService
    {
        public Kernel Kernel { get; set; }

        public Task<ChatMessageContent> SendMessageToKernelAsync(string message);
    }
}
