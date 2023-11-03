using Microsoft.AspNetCore.Identity;
using RChat.Application.Contracts.Users;
using RChat.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Application.Users
{
    public class UserService : IUserService
    {
        private UserManager<User> _userManager;
        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager; 
        }
        public async Task<bool> ChangeUserPasswordAsync(string userEmail, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
                return false;

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            return result.Succeeded;

        }
    }
}
