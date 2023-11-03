using Microsoft.AspNetCore.Identity;
using RChat.Domain.Chats;
using RChat.Domain.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Domain.Users
{
    // test class
    public class User : IdentityUser<int>
    {       
        public ICollection<Chat> Chats { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<Chat> CreatedChats { get; set; }
    }
}
