using RChat.Domain.Repsonses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Domain.Users.DTO
{
    public class UserTokenResponse : ApiResponse
    {
        public string? Token { get; set; }
    }
}
