using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Domain.Users.DTO
{
    public class LoginUserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
