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
            var query = await chatRepository.GetAllAsQueryableAsync();
            var infoQuery = query.Select(c =>
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
    }
}
