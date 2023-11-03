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
    }
}
