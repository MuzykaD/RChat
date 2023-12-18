using RChat.Domain.Chats;
using RChat.Domain.Chats.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Application.Mappers
{
    public static class ChatMapper
    {
        public static ChatDto ToChatDto(this Chat chat)
        {
            return new ChatDto()
            {
                Id = chat.Id,
                Name = chat.Name,
                CreatorId = chat.CreatorId,
                Users = chat.Users.Select(u => u.ToUserInformationDto()).ToHashSet(),
                Messages = chat.Messages.Select(m => m.ToMessageInformationDto()).ToList(),
                IsGroupChat = chat.IsGroupChat,
                Assistant = chat.Assistant
            };
        }
    }
}
