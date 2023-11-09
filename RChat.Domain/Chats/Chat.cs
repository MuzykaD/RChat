using RChat.Domain.Common;
using RChat.Domain.Messages;
using RChat.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Domain.Chats
{
    public class Chat : IDbEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CreatorId { get; set; }
        public User? Creator { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Message> Messages { get; set; }

    }
}
