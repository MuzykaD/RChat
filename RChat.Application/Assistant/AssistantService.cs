using RChat.Application.Contracts.Assistant;
using RChat.Application.Contracts.Chats;
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
        public async Task<List<Domain.Assistants.Assistant>> GetAvailableAssistantsAsync(int userId)
        {
            var chatRepo = _unitOfWork.GetRepository<Chat, int>();
            var groupIdentifiers = chatRepo.GetAllIncluding(c => c.Users).Where(c => c.Users.Any(u => u.Id.Equals(userId))).Select(c => c.Id).ToList();

            var assistantRepo = _unitOfWork.GetRepository<Domain.Assistants.Assistant, string>();

            var assistants =  assistantRepo.GetAllIncluding(a => a.AssistantFiles).Where(a => groupIdentifiers.Contains(a.ChatId)).ToList();

            return await Task.FromResult(assistants);
        }
    }
}
