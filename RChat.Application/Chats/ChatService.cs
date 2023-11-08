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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Application.Chats
{
    public class ChatService : IChatService
    {
        private IQueryBuilder<Chat> _chatQueryBuilder;
        private IUnitOfWork _unitOfWork;

        public ChatService(IQueryBuilder<Chat> queryBuilder, IUnitOfWork unitOfWork)
        {
            _chatQueryBuilder = queryBuilder;
            _unitOfWork = unitOfWork;
        }

        public async Task<GridListDto<ChatInformationDto>> GetChatsInformationListAsync(SearchArguments searchArguments)
        {
            var chatRepository = await _unitOfWork.GetRepositoryAsync<Chat>();
            var query = await chatRepository.GetAllAsQueryableAsync();
            if(searchArguments.SearchRequired)           
                query = _chatQueryBuilder.BuildSearchQuery(query ,searchArguments.Value!);

            if(searchArguments.OrderByRequired)
                query = _chatQueryBuilder.BuildOrderByQuery(query, searchArguments.OrderBy!, searchArguments.OrderByType!);

            var totalCount = query.Count();
            var chatInfo = 
                query
                .Skip(searchArguments.Skip)
                .Take(searchArguments.Take)
                .Select(c =>
                  new ChatInformationDto()
                  {
                      Name = c.Name,
                      CreatorId = c.CreatorId,
                      MessagesCount = c.Messages.Count(),
                      UsersCount = c.Users.Count()
                  }
            ).ToList();
            return new GridListDto<ChatInformationDto>()
            {
                SelectedEntities = chatInfo,
                TotalCount = totalCount
            };
        }
    }
}
