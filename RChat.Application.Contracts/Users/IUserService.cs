using RChat.Domain.Repsonses;
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
        Task<bool> ChangeUserPasswordAsync(string userEmail, string currentPassword, string newPassword);
        Task<UserInformationDto> GetPersonalInformationAsync(string userEmail);

        Task<bool> UpdateUserAsync(string userEmail,UpdateUserDto updateDto);
        
    }
}
