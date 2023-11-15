using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Domain.Common
{
    public class NotificationArguments
    {
        public int ReferenceId { get; set; }
        public bool IsGroup { get; set; }
        public string ChatName { get; set; }
    }
}
