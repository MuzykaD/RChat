using RChat.Application.Common;
using RChat.Application.Contracts.Common;
using RChat.Application.Contracts.Messages;
using RChat.Application.Mappers;
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
        //private IQueryBuilder<Message> _messageQueryBuilder;
        private IUnitOfWork _unitOfWork;

        public MessageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateMessageAsync(int senderId, MessageInformationDto message)
        {
            var messageRepository = _unitOfWork.GetRepository<Message, int>();
            try
            {
               await messageRepository.CreateAsync(new()
                {
                    SenderId = senderId,
                    Content = message.Content,
                    ChatId = message.ChatId,
                    SentAt = message.SentAt,
                });
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }


        }

        public async Task<GridListDto<MessageInformationDto>> GetMessagesInformationListAsync(SearchArguments searchArguments)
        {
            var messageRepository = _unitOfWork.GetRepository<Message, int>();
            var query = messageRepository.GetAllAsQueryable();
            if (searchArguments.SearchRequired)
                query = QueryBuilder<Message>.BuildSearchQuery(query, searchArguments.Value!);

            if (searchArguments.OrderByRequired)
                query = QueryBuilder<Message>.BuildOrderByQuery(query, searchArguments.OrderBy!, searchArguments.OrderByType!);

            var totalCount = query.Count();
            var chatInfo =
                query
                .Skip(searchArguments.Skip)
                .Take(searchArguments.Take)
                .Select(c => c.ToMessageInformationDto()).ToList();
            return new GridListDto<MessageInformationDto>()
            {
                SelectedEntities = chatInfo,
                TotalCount = totalCount
            };
        }
    }
}
