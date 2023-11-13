using RChat.Application.Common;
using RChat.Application.Contracts.Chats;
using RChat.Application.Contracts.Common;
using RChat.Domain;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Dto;
using RChat.Domain.Repsonses;
using RChat.Domain.Users;
using RChat.Infrastructure.Contracts.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Application.Chats
{
    public class ChatService : IChatService
    {
        private IUnitOfWork _unitOfWork;

        public ChatService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //TODO order by DTO fields
        public async Task<GridListDto<ChatInformationDto>> GetChatsInformationListAsync(SearchArguments searchArguments)
        {
            var chatRepository = _unitOfWork.GetRepository<Chat, int>();
            var infoQuery = chatRepository
                .GetAllAsQueryable()
                .Select(c =>
            new ChatInformationDto()
            {
                Id = c.Id,
                Name = c.Name,
                CreatorName = c.Creator == null ? null : c.Creator.UserName,
                UsersCount = c.Users.Count()
            });
            if(searchArguments.SearchRequired)
                infoQuery = QueryBuilder<ChatInformationDto>.BuildSearchQuery(infoQuery, searchArguments.Value!);
            var totalCount = infoQuery.Count();
            if (searchArguments.OrderByRequired)
                infoQuery = QueryBuilder<ChatInformationDto>.BuildOrderByQuery(infoQuery, searchArguments.OrderBy!, searchArguments.OrderByType!);
                      
            return new GridListDto<ChatInformationDto>()
            {
                SelectedEntities = infoQuery.Skip(searchArguments.Skip).Take(searchArguments.Take).ToList(),
                TotalCount = totalCount
            };
        }

        public async Task<Chat?> GetPrivateChatByEmailsAsync(string firstUserEmail, string secondUserEmail)
        {
            var chatRepository = _unitOfWork.GetRepository<Chat, int>();
            var requiredChat = chatRepository.GetAllIncluding(c => c.Users, c => c.Messages).FirstOrDefault(
                c => !c.IsGroupChat && 
                c.Users.All(u => u.Email == firstUserEmail || u.Email == secondUserEmail));
           
            if(requiredChat == null)
            {
                var newChat = new Chat()
                {
                    IsGroupChat = false,
                    Name = $"Private {secondUserEmail}",
                    Users = _unitOfWork.GetRepository<User, int>()
                    .GetAllAsQueryable()
                    .Where(u => u.Email == firstUserEmail || u.Email == secondUserEmail).ToList()
                };
                await chatRepository.CreateAsync(newChat);
                await _unitOfWork.SaveChangesAsync();
                return newChat;
            }
            return requiredChat;
        }
    }
}
