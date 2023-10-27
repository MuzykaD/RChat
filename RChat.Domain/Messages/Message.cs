using RChat.Domain.Attachments;
using RChat.Domain.Chats;
using RChat.Domain.Users;

namespace RChat.Domain.Messages
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ChatId { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
        public virtual Chat Chat { get; set; }
        public virtual User Sender { get; set; }
    }
}
