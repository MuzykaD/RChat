using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Domain.Messages.Dto
{
    public class MessageInformationDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
        public int SenderId { get; set; }
        public string? SenderEmail { get; set; }
        public int ChatId { get; set; }
    }
}
