﻿using RChat.Application.Contracts.Common;
using RChat.Application.Contracts.Messages;
using RChat.Domain;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Dto;
using RChat.Domain.Messages;
using RChat.Domain.Messages.Dto;
using RChat.Domain.Repsonses;
using RChat.Infrastructure.Contracts.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Application.Messages
{


    public class MessageService : IMessageService
    {
        private IQueryBuilder<Message> _messageQueryBuilder;
        private IUnitOfWork _unitOfWork;

        public MessageService(IQueryBuilder<Message> queryBuilder, IUnitOfWork unitOfWork)
        {
            _messageQueryBuilder = queryBuilder;
            _unitOfWork = unitOfWork;
        }
        public async Task<GridListDto<MessageInformationDto>> GetMessagesInformationListAsync(SearchArguments searchArguments)
        {
            var messageRepository = await _unitOfWork.GetRepositoryAsync<Message>();
            var query = await messageRepository.GetAllAsQueryableAsync();
            if (searchArguments.SearchRequired)
                query = _messageQueryBuilder.BuildSearchQuery(query, searchArguments.Value!);

            if (searchArguments.OrderByRequired)
                query = _messageQueryBuilder.BuildOrderByQuery(query, searchArguments.OrderBy!, searchArguments.OrderByType!);

            var totalCount = query.Count();
            var chatInfo =
                query
                .Skip(searchArguments.Skip)
                .Take(searchArguments.Take)
                .Select(c =>
                  new MessageInformationDto()
                  {
                      Id = c.Id,
                      SenderId = c.SenderId,
                      Content = c.Content,
                      ChatId = c.ChatId,
                      SentAt = c.SentAt,
                  }
            ).ToList();
            return new GridListDto<MessageInformationDto>()
            {
                SelectedEntities = chatInfo,
                TotalCount = totalCount
            };
        }
    }
}
