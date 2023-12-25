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
    public class AssistantMessageService : BaseAssistantService, IAssistantMessageService
    {
        public AssistantMessageService(IConfiguration configuration)
        {
            _assistantApiKey = configuration["AssistantsApiKey"]!;
            OpenAIClient = new OpenAIClient(_assistantApiKey);
        }
        public async Task<string> SendMessageAndGetResponseAsync(string message)
        {
            if (string.IsNullOrWhiteSpace(_currentThreadId))
                await CreateThreadAsync();
            var messageCreateParameter = new MessageCreateParameter
            {
                Thread_Id = _currentThreadId!,
                Role = "user",
                Content = message
            };
            var assistantMessage = await OpenAIClient.MessageCreateAsync(messageCreateParameter);

            var runCreateParameter = new RunCreateParameter
            {
                Thread_Id = _currentThreadId!,
                Assistant_Id = _currentAssistantId!
            };
            var run = await OpenAIClient.RunCreateAsync(runCreateParameter);           
            return await GetAssistantResponseAsync(_currentThreadId!, run.Id);    
        }

        private async Task<string> GetAssistantResponseAsync(string threadId, string runId)
        {
            var runRetrieveParameter = new RunRetrieveParameter()
            {
                Thread_Id = threadId,
                Run_Id = runId
            };
            int counter = 0;
            while (counter <= 10)
            {
                var result = await OpenAIClient.RunRetrieveAsync(runRetrieveParameter);
                if (result.Status != "queued" &&
                    result.Status != "in_progress" &&
                    result.Status != "cancelling")
                {
                    var p1 = new MessagesParameter();
                    p1.Thread_Id = threadId;
                    p1.QueryParameter.Order = "desc";
                    var res1 = await OpenAIClient.MessagesAsync(p1);
                    foreach (var item in res1.Data)
                    {
                        foreach (var content in item.Content)
                        {
                            if (content.Text == null) { continue; }
                            return content.Text.Value;
                        }
                    }
                }
                else
                { 
                    await Task.Delay(500);
                    counter++;
                }
            }
            return "Error occured";

        }

        public async Task InitializeAsync(string assistantId)
        {
            var all = await OpenAIClient.AssistantsAsync();
            _currentAssistantId = all.Data.FirstOrDefault(a => a.Id == assistantId)!.Id;
            if (_currentAssistantId is null)
                throw new ArgumentException("Assistant for this chat is not created yet");
            else
                IsInitialized = true;
        }


    }
}
