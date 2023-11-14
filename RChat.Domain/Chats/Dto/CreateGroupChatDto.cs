using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Domain.Chats.Dto
{
    public class CreateGroupChatDto
    {
        public ICollection<int> MembersId { get; set; }
        public string GroupName { get; set; }
    }
}
