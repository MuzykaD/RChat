﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Domain.Users.DTO
{
    public class UpdateUserDto
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string? PhoneNumber { get; set; }

    }
}
