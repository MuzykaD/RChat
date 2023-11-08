using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Domain.Chats.Dto
{
    public class ChatInformationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CreatorId{ get; set; }
        public int UsersCount { get; set; }
        public int MessagesCount { get; set; }
    }
}
