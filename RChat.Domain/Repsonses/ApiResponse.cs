using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Domain.Repsonses
{
    public class ApiResponse
    {
        public bool IsSucceed { get; set; }
        public string? Message { get; set; }
    }
}
