using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Domain.Messages.Dto
{
    public class CreateMessageDto
    {
        public int ChatId { get; set; }
        public string MessageValue { get; set; }
    }
}
