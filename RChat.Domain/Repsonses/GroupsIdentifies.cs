using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Domain.Repsonses
{
    public class GroupsIdentifies : ApiResponse
    {
        public List<int> SignalIdentifiers { get; set; }
    }
}
