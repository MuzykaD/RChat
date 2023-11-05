using Microsoft.AspNetCore.Identity;
using RChat.Application.Contracts.Users;
using RChat.Domain.Repsonses;
using RChat.Domain.Users;
using RChat.Domain.Users.DTO;
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
        public async Task<GridListDto<UserInformationDto>> GetUsersInformationListAsync(string? value, int skip = 0, int takeCount = 5)
        {
            var users = _userManager.Users;
            if(!string.IsNullOrWhiteSpace(value))
            {
                users = users.Where(u =>
                u.Email!.Contains(value)
                || u.UserName!.Contains(value)
                || (u.PhoneNumber ?? string.Empty).Contains(value));
            }
            var totalCount = users.Count();
            var userInformation = users.Skip(skip).Take(takeCount).Select(u => new UserInformationDto()
            {
                Email = u.Email,
                UserName = u.UserName,
                PhoneNumber = u.PhoneNumber
            });

            return await Task.FromResult(new GridListDto<UserInformationDto>()
            {
                SelectedEntities = userInformation,
                TotalCount = totalCount,
            });
        }

        public async Task<IEnumerable<UserInformationDto>> SearchUsersInformationListAsync(string searchValue)
        {
            var userInformation = _userManager.Users
                .Where(u =>
                u.Email!.Contains(searchValue)
                || u.UserName!.Contains(searchValue)
                || (u.PhoneNumber ?? string.Empty).Contains(searchValue))
                .Select(u => new UserInformationDto()
                {
                    Email = u.Email!,
                    UserName = u.UserName!,
                    PhoneNumber = u.PhoneNumber
                });

            return await Task.FromResult(userInformation);
        }
    }
}
