using Microsoft.AspNetCore.Identity;
using RChat.Application.Contracts.Account;
using RChat.Application.Mappers;
using RChat.Domain.Repsonses;
using RChat.Domain.Users;
using RChat.Domain.Users.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.Application.Account
{
    public class AccountService : IAccountService
    {
        private UserManager<User> _userManager;
        public AccountService(UserManager<User> userManager)
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

        public async Task<UserInformationDto> GetPersonalInformationAsync(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            return user.ToUserInformationDto();
        }

        public async Task<bool> UpdateUserAsync(string userEmail, UpdateUserDto updateDto)
        {
            var currentUser = await _userManager.FindByEmailAsync(userEmail);
            var userByProvidedEmail = await _userManager.FindByEmailAsync(updateDto.Email);
            if (currentUser == null ||
                (userByProvidedEmail != null && userEmail != userByProvidedEmail.Email))
                return false;

            currentUser.UserName = updateDto.UserName;
            currentUser.PhoneNumber = updateDto.PhoneNumber;
            currentUser.Email = updateDto.Email;

            var updateResult = await _userManager.UpdateAsync(currentUser);
            return updateResult.Succeeded;
        }
    }
}
