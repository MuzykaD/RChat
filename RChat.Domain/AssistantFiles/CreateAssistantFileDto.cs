using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Domain.AssistantFiles
{
    public class CreateAssistantFileDto
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string AssistantId { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
