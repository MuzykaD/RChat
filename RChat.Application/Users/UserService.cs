using Microsoft.AspNetCore.Identity;
using RChat.Application.Common;
using RChat.Application.Contracts.Common;
using RChat.Application.Contracts.Users;
using RChat.Domain;
using RChat.Domain.Repsonses;
using RChat.Domain.Users;
using RChat.Domain.Users.DTO;
using System.Linq.Dynamic.Core;


namespace RChat.Application.Users
{
    public class UserService : IUserService
    {
        private UserManager<User> _userManager;
        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<GridListDto<UserInformationDto>> GetUsersInformationListAsync(SearchArguments searchArguments)
        {
            var users = _userManager.Users.AsQueryable();
            if (searchArguments.SearchRequired)
                users = QueryBuilder<User>.BuildSearchQuery(users, searchArguments.Value!);

            if (searchArguments.OrderByRequired)
                users = QueryBuilder<User>.BuildOrderByQuery(users, searchArguments.OrderBy!, searchArguments.OrderByType!);

            var totalCount = users.Count();

            var userInformation =
                users
                .Skip(searchArguments.Skip)
                .Take(searchArguments.Take)
                .Select(u => new UserInformationDto()
                {
                    Email = u.Email!,
                    UserName = u.UserName!,
                    PhoneNumber = u.PhoneNumber
                }).ToList();

            return await Task.FromResult(new GridListDto<UserInformationDto>()
            {
                SelectedEntities = userInformation,
                TotalCount = totalCount,
            });
        }
    }
}
