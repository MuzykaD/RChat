using RChat.Domain.AssistantFiles;
using RChat.Domain.Chats;
using RChat.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Domain.Assistants
{
    public class Assistant : IDbEntity<string>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Instructions { get; set; }
        public int ChatId { get; set; }
        public Chat Chat { get; set; }
        public  ICollection<AssistantFile> AssistantFiles { get; set; } = new List<AssistantFile>();
    }
}
