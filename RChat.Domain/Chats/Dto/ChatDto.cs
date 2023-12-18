using RChat.Domain.Assistants;
using RChat.Domain.Messages.Dto;
using RChat.Domain.Users.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Domain.Chats.Dto
{
    public class ChatDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CreatorId { get; set; }
        public HashSet<UserInformationDto> Users { get; set; }
        public List<MessageInformationDto> Messages { get; set; }
        public bool IsGroupChat { get; set; }
        public Assistant Assistant { get; set; }
    }
}
