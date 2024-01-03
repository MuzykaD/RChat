using RChat.Domain.Assistants;
using RChat.Domain.Common;
using RChat.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Domain.AssistantFiles
{
    public class AssistantFile : IDbEntity<string>
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; }
        public string AssistantId { get; set; }
        public Assistant Assistant { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
