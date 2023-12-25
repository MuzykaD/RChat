
using RChat.Domain.AssistantFiles;
using RChat.Domain.Assistants;
using RChat.Domain.Assistants.Dto;
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
        Task UpdateAssistantAsync(string assistantId, AssistantUpdateDto assistantUpdateDto);
        Task<AssistantInfoDto> GetAssistantInfoByIdAsync(string assistantId);
        public Task<List<RChat.Domain.Assistants.Assistant>> GetAvailableAssistantsAsync(int userId);
        public Task<List<AssistantFile>> GetFilesByAssistantIdAsync(string assistantId);
    }
}
