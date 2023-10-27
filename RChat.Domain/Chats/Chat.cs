using RChat.Domain.Messages;
using RChat.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Domain.Chats
{
    public class Chat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CreatorId { get; set; }
        public User? Creator { get; set; }
        public List<User> Users { get; set; }
        public List<Message> Messages { get; set; }

    }
}
