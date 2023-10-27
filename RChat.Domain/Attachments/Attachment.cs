using RChat.Domain.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Domain.Attachments
{
    public class Attachment
    {
        public int Id { get; set; }

        public int MessageId { get; set; }

        public string FileName { get; set; }

        public string FileUrl { get; set; }

        public virtual Message Message { get; set; }
    }
}
