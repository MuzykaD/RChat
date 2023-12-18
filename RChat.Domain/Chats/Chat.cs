using RChat.Domain.Assistants;
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
        public virtual User? Creator { get; set; }
        public bool IsGroupChat { get; set; }
        public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
        public Assistant Assistant { get; set; }
    }
}
