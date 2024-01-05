using HigLabo.OpenAI;
using Microsoft.Extensions.Configuration;
using RChat.Application.Contracts.Assistant;
using RChat.Domain.Shopping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RChat.Application.Assistant
{
    public class ShoppingAssistantService : BaseAssistantService, IShoppingAssistantService
    {
        public ShoppingAssistantService(IConfiguration configuration)
        {
            _assistantApiKey = configuration["AssistantsApiKey"]!;
            OpenAIClient = new OpenAIClient(_assistantApiKey);
        }

        public async Task<List<ShoppingProduct>> GetListOfShoppingProductsAsync(string htmlCards)
        {
            var createMessageParam = new MessageCreateParameter()
            {
                Thread_Id = _currentThreadId!,
                Role = "user",
                Content = htmlCards
            };
            if (string.IsNullOrWhiteSpace(_currentThreadId))
                await CreateThreadAsync();

            var assistantMessage = await OpenAIClient.MessageCreateAsync(createMessageParam);

            var runCreateParameter = new RunCreateParameter
            {
                Thread_Id = _currentThreadId!,
                Assistant_Id = _currentAssistantId!
            };
            var run = await OpenAIClient.RunCreateAsync(runCreateParameter);
            var result = await GetAssistantResponseAsync(_currentThreadId!, run.Id);
            return null;
        }

        public async Task<List<ShoppingProduct>> GetListOfShoppingProductsAsync(string[] htmlCards)
        {
            var results = new List<ShoppingProduct>();          
            var tasks = new List<Task<string>>();

            foreach (var htmlCard in htmlCards)
            {
                if (string.IsNullOrWhiteSpace(_currentThreadId))
                    await CreateThreadAsync();
                var threadId = _currentThreadId!;
                tasks.Add(Task.Run(async () =>
                {
                    
                    var createMessageParam = new MessageCreateParameter()
                    {
                        Thread_Id = threadId!,
                        Role = "user",
                        Content = htmlCard
                    };
                    var assistantMessage = await OpenAIClient.MessageCreateAsync(createMessageParam);

                    var runCreateParameter = new RunCreateParameter
                    {
                        Thread_Id = threadId!,
                        Assistant_Id = _currentAssistantId!
                    };
                    var run = await OpenAIClient.RunCreateAsync(runCreateParameter);
                    var result = await GetAssistantResponseAsync(threadId!, run.Id);

                    return result;
                }));
                _currentThreadId = null;
            }

            await Task.WhenAll(tasks);
            
            foreach (var task in tasks)
            {
                var assistantResponse = await task;
                //TODO normal json parsing
                int startIndex = assistantResponse.IndexOf('{');
                int endIndex = assistantResponse.LastIndexOf('}');
                string jsonContent = assistantResponse.Substring(startIndex, endIndex - startIndex + 1);
                results.Add(JsonSerializer.Deserialize<ShoppingProduct>(jsonContent)!);
            }

            return results;
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

        private async Task<string> GetAssistantResponseAsync(string threadId, string runId)
        {
            var runRetrieveParameter = new RunRetrieveParameter()
            {
                Thread_Id = threadId,
                Run_Id = runId
            };
            int counter = 0;
            while (true)
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
        }


    }
}
