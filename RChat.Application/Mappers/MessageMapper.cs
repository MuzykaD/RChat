using RChat.Domain.Messages;
using RChat.Domain.Messages.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Application.Mappers
{
    public static class MessageMapper
    {
        public static MessageInformationDto ToMessageInformationDto(this Message message) 
        {
            return new()
            {
                Id = message.Id,
                Content = message.Content,
                ChatId = message.ChatId,
                SenderId = message.SenderId,
                SentAt = message.SentAt,
            };
        }
    }
}
