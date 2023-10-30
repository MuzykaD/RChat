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
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public  ICollection<Chat> Chats { get; set; }
        public  ICollection<Message> Messages { get; set; }
        public  ICollection<Chat> ChatsNavigation { get; set; }
    }
}
