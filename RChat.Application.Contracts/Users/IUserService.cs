using RChat.Domain.Repsonses;
using RChat.Domain.Users;
using RChat.Domain.Users.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Application.Contracts.Users
{
    public interface IUserService
    {
        Task<GridListDto<UserInformationDto>> GetUsersInformationListAsync(string? value,int skip = 0, int takeCount = 5, string? orderBy = null, string? orderByType = null);
    }
}
