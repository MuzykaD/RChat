using RChat.Domain.Repsonses;
using RChat.Domain.Users.DTO;
using RChat.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RChat.Domain.Chats.Dto;
using RChat.Domain.Chats;

namespace RChat.Application.Contracts.Chats
{
    public interface IChatService
    {
        Task<GridListDto<ChatInformationDto>> GetChatsInformationListAsync(SearchArguments searchArguments);

        Task<Chat?> GetPrivateChatByEmailsAsync(string firstUserEmail, string secondUserEmail);

    }
}
