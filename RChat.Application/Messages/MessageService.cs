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
using RChat.Infrastructure.Contracts.Common;
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
        private IUnitOfWork _unitOfWork;
        private IRepository<Message, int> _messageRepository;

        public MessageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _messageRepository = _unitOfWork.GetRepository<Message, int>();
        }

        public async Task<int?> CreateMessageAsync(int senderId, MessageInformationDto message)
        {          
                Message newMessage = new()
                {
                    SenderId = senderId,
                    Content = message.Content,
                    ChatId = message.ChatId,
                    SentAt = message.SentAt,
                    IsAssistantGenerated = message.IsAssistnatGenerated
                };

                await _messageRepository.CreateAsync(newMessage);
                await _unitOfWork.SaveChangesAsync();
                return newMessage.Id;
        }

        public async Task<bool> DeleteMessageAsync(int messageId, int currentUserId)
        {

            var message = await _messageRepository.GetByIdAsync(messageId);
            if (message.SenderId == currentUserId)
            {
                await _messageRepository.DeleteAsync(messageId);
                return true;
            }
            else
                return false;
        }

        public async Task<GridListDto<MessageInformationDto>> GetMessagesInformationListAsync(SearchArguments searchArguments)
        {
            var query = _messageRepository.GetAllIncluding(c => c.Chat, c => c.Sender);
            if (searchArguments.SearchRequired)
                query = QueryBuilder<Message>.BuildSearchQuery(query, searchArguments.Value!);

            if (searchArguments.OrderByRequired)
                query = QueryBuilder<Message>.BuildOrderByQuery(query, searchArguments.OrderBy!, searchArguments.OrderByType!);

            var totalCount = query.Count();
            var chatInfo = query
                .Skip(searchArguments.Skip)
                .Take(searchArguments.Take)
                .Select(c => c.ToMessageInformationDto()).ToList();

            return await Task.FromResult(new GridListDto<MessageInformationDto>()
            {
                SelectedEntities = chatInfo,
                TotalCount = totalCount
            });
        }

        public async Task<bool> UpdateMessageAsync(int currentUserId, MessageInformationDto message)
        {
            var messageToUpdate = await _messageRepository.GetByIdAsync(message.Id);

            if(messageToUpdate == null)
                return false;

            if(messageToUpdate.Id == message.Id && messageToUpdate.SenderId == message.SenderId && message.SenderId == currentUserId)
            {
                messageToUpdate.Content = message.Content;

                _messageRepository.Update(messageToUpdate);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            return false;  
        }
    }
}
