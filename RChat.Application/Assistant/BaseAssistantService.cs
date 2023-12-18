using HigLabo.OpenAI;
using RChat.Application.Contracts.Assistant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Application.Assistant
{
    public abstract class BaseAssistantService
    {
        protected OpenAIClient OpenAIClient { get; set; }
        protected string _assistantApiKey;
        protected string? _currentThreadId;
        protected string? _currentAssistantId;
        protected bool IsInitialized;

        //Use this method to fetch the right assistant
        protected async Task CreateThreadAsync()
        {
            var threadResult = await OpenAIClient.ThreadCreateAsync();
            _currentThreadId = threadResult.Id;
        }
    }
}
