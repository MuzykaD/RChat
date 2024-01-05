using HigLabo.OpenAI;
using Microsoft.Extensions.Configuration;
using RChat.Application.Contracts.Assistant;
using RChat.Application.Contracts.Chats;
using RChat.Application.Mappers;
using RChat.Domain.AssistantFiles;
using RChat.Domain.Assistants.Dto;
using RChat.Domain.Chats;
using RChat.Domain.Users;
using RChat.Infrastructure.Contracts.Common;
using RChat.Infrastructure.Contracts.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Application.Assistant
{
    public class AssistantService : IAssistantService
    {
        private IUnitOfWork _unitOfWork;
        private OpenAIClient OpenAIClient { get; set; }
        private IRepository<Chat, int> _chatRepository;
        private IRepository<AssistantFile, string> _assistantFileRepository;
        private IRepository<Domain.Assistants.Assistant, string> _assistantRepository;
        public AssistantService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _chatRepository = _unitOfWork.GetRepository<Chat, int>();
            _assistantFileRepository = _unitOfWork.GetRepository<AssistantFile, string>();
            _assistantRepository = _unitOfWork.GetRepository<Domain.Assistants.Assistant, string>();
            OpenAIClient = new OpenAIClient(configuration["AssistantsApiKey"]!);
        }

        public async Task CreateAssitantFileAsync(int currentUserId, CreateAssistantFileDto createFileDto)
        {

            var newFile = new AssistantFile()
            {
                Id = createFileDto.Id,
                Name = createFileDto.Name,
                AssistantId = createFileDto.AssistantId,
                CreatedAt = createFileDto.CreatedAt,
                UserId = currentUserId
            };
            await _assistantFileRepository.CreateAsync(newFile);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAssitantFileAsync(string fileId)
        {
            await _assistantFileRepository.DeleteAsync(fileId);
        }

        public async Task UpdateAssistantAsync(string assistantId,AssistantUpdateDto assistantUpdateDto)
        {
            //db update
            var assistant = await _assistantRepository.GetByIdAsync(assistantId);
            assistant.Instructions = assistantUpdateDto.Instructions;
            assistant.Name = assistantUpdateDto.Name;
            _assistantRepository.Update(assistant);

            await _unitOfWork.SaveChangesAsync();

            //open ai data update
            var updateParam = new AssistantModifyParameter()
            {
                Assistant_Id = assistantId,
                Name = assistantUpdateDto.Name,
                Instructions = assistantUpdateDto.Instructions,
            };
            await OpenAIClient.AssistantModifyAsync(updateParam);
        }

        public async Task<AssistantInfoDto> GetAssistantInfoByIdAsync(string assistantId)
        {
            var assistant = _assistantRepository.GetAllAsQueryable()
                                          .Where(a => a.Id.Equals(assistantId))
                                          .Select(a => new AssistantInfoDto()
                                          {
                                              Name = a.Name,
                                              Context = a.Instructions,
                                              ChatName = a.Chat.Name,
                                              FilesCount = a.AssistantFiles.Count
                                          });
            return await Task.FromResult(assistant.First());
        }

        public async Task<List<Domain.Assistants.Assistant>> GetAvailableAssistantsAsync(int userId)
        {
            var groupIdentifiers = _chatRepository.GetAllIncluding(c => c.Users).Where(c => c.Users.Any(u => u.Id.Equals(userId))).Select(c => c.Id).ToList();


            var assistants = _assistantRepository.GetAllIncluding(a => a.AssistantFiles).Where(a => groupIdentifiers.Contains(a.ChatId)).ToList();

            return await Task.FromResult(assistants);
        }

        public async Task<List<AssistantFile>> GetFilesByAssistantIdAsync(string assistantId)
        {
            var result = await _assistantFileRepository.GetAllAsync(af => af.AssistantId.Equals(assistantId));
            return result.ToList();
        }
    }
}
