using HigLabo.OpenAI;
using Microsoft.Extensions.Configuration;
using RChat.Application.Contracts.Assistant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Application.Assistant
{
    public class AssistantFactory : IAssistantFactory
    {
        private OpenAIClient OpenAIClient { get; set; }
        public AssistantFactory(IConfiguration configuration)
        {
            var assistantApiKey = configuration["AssistantsApiKey"]!;
            OpenAIClient = new OpenAIClient(assistantApiKey);
        }       
        public async Task<AssistantCreateResponse> CreateAssistantForGroupAsync(string groupName)
        {
            var createParam = new AssistantCreateParameter()
            {
                Name = groupName + "-Assistant",
                Instructions = "Just chat with me",
                Model = "gpt-3.5-turbo-1106",
                Tools = new() { new("code_interpreter"), new("retrieval") }
            };
            return await OpenAIClient.AssistantCreateAsync(createParam);
           
        }
    }
}
