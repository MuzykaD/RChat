﻿using RChat.Domain.Users.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Application.Contracts.Users
{
    public interface IUserService
    {
        Task<IEnumerable<UserInformationDto>> GetUsersInformationListAsync();
        Task<IEnumerable<UserInformationDto>> SearchUsersInformationListAsync(string searchValue);
    }
}
