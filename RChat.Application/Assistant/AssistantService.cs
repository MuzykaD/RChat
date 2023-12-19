using RChat.Application.Contracts.Assistant;
using RChat.Application.Contracts.Chats;
using RChat.Domain.AssistantFiles;
using RChat.Domain.Chats;
using RChat.Infrastructure.Contracts.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Application.Assistant
{
    public class AssistantService : IAssistantService
    {
        private IUnitOfWork _unitOfWork;
        public AssistantService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAssitantFileAsync(CreateAssistantFileDto createFileDto)
        {
            var filesRepo = _unitOfWork.GetRepository<AssistantFile, string>();
            var newFile = new AssistantFile()
            {
                Id = createFileDto.Id,
                Name = createFileDto.Name,
                AssistantId = createFileDto.AssistantId,
                CreatedAt = createFileDto.CreatedAt,
            };
            await filesRepo.CreateAsync(newFile);  
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAssitantFileAsync(string fileId)
        {
            var filesRepo = _unitOfWork.GetRepository<AssistantFile, string>();
            await filesRepo.DeleteAsync(fileId);
        }

        public async Task<List<Domain.Assistants.Assistant>> GetAvailableAssistantsAsync(int userId)
        {
            var chatRepo = _unitOfWork.GetRepository<Chat, int>();
            var groupIdentifiers = chatRepo.GetAllIncluding(c => c.Users).Where(c => c.Users.Any(u => u.Id.Equals(userId))).Select(c => c.Id).ToList();

            var assistantRepo = _unitOfWork.GetRepository<Domain.Assistants.Assistant, string>();

            var assistants =  assistantRepo.GetAllIncluding(a => a.AssistantFiles).Where(a => groupIdentifiers.Contains(a.ChatId)).ToList();

            return await Task.FromResult(assistants);
        }

        public async Task<List<AssistantFile>> GetFilesByAssistantIdAsync(string assistantId)
        {
            var filesRepo = _unitOfWork.GetRepository<AssistantFile, string>();

            var result = await filesRepo.GetAllAsync(af => af.AssistantId.Equals(assistantId));
            return result.ToList();
        }
    }
}
