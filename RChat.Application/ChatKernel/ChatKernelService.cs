using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using RChat.Application.ChatKernelPlugins;
using RChat.Application.Contracts.ChatKernel;
using RChat.Domain.Users;
using RChat.Infrastructure.Contracts.UnitOfWork;
using System.Security.Claims;

namespace RChat.Application.ChatKernel
{
    public class ChatKernelService : IChatKernelService
    {
      Task
        public Kernel Kernel { get; set; }
        public ChatKernelService(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            var builder = Kernel.CreateBuilder();
            builder.AddOpenAIChatCompletion("gpt-3.5-turbo", configuration["ApiKey"]!);
            //builder.Plugins.AddFromType<ChatKernelPlugin>();
            builder.Plugins.AddFromObject(new ChatKernelPlugin(unitOfWork));
            Kernel = builder.Build();
        }

        public async Task<ChatMessageContent> SendMessageToKernelAsync(string message)
        {
            ChatHistory history = new();
            var chatCompletionService = Kernel.GetRequiredService<IChatCompletionService>();
            OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
                MaxTokens = 1000
            };
            history.AddUserMessage(message);
            var result = await chatCompletionService.GetChatMessageContentAsync(
                history,
                executionSettings: openAIPromptExecutionSettings,
                kernel: Kernel);
            history.AddMessage(result.Role, result.Content);
            return result;
        }
    }
}
