using RChat.Domain.Attachments;
using RChat.Domain.Chats;
using RChat.Domain.Users;

namespace RChat.Domain.Messages
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
        public int SenderId { get; set; }
        public int ChatId { get; set; }
        public  ICollection<Attachment> Attachments { get; set; }
        public  Chat Chat { get; set; }
        public  User Sender { get; set; }
    }
}
