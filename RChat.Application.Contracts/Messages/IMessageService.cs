using RChat.Domain.Chats.Dto;
using RChat.Domain.Repsonses;
using RChat.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RChat.Domain.Messages.Dto;

namespace RChat.Application.Contracts.Messages
{
    public interface IMessageService
    {
        Task<GridListDto<MessageInformationDto>> GetMessagesInformationListAsync(SearchArguments searchArguments);
        Task<int?> CreateMessageAsync(int senderId, MessageInformationDto message);
        Task<bool> DeleteMessageAsync(int messageId, int currentUserId);
    }
}
