using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Domain.Assistants.Dto
{
    public class AssistantInfoDto
    {
        public string Name { get; set; } = "Loading";
        public string ChatName { get; set; }  = "Loading";
        public string Context { get; set; } = "Loading";
        public int FilesCount { get; set; }

    }
}
