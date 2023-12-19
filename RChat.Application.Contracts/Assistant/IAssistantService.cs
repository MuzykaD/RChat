
using RChat.Domain.AssistantFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Application.Contracts.Assistant
{
    public interface IAssistantService
    {
        Task CreateAssitantFileAsync(CreateAssistantFileDto createFileDto);
        Task DeleteAssitantFileAsync(string fileId);
        public Task<List<RChat.Domain.Assistants.Assistant>> GetAvailableAssistantsAsync(int userId);
        public Task<List<AssistantFile>> GetFilesByAssistantIdAsync(string assistantId);
    }
}
