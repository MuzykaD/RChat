using Microsoft.AspNetCore.Identity;
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
        private IQueryBuilder<User> _userQueryBuilder;
        public UserService(UserManager<User> userManager, IQueryBuilder<User> userQueryBuilder)
        {
            _userManager = userManager;
            _userQueryBuilder = userQueryBuilder;
        }
        public async Task<GridListDto<UserInformationDto>> GetUsersInformationListAsync(SearchArguments searchArguments)
        {
            var users = _userManager.Users.AsQueryable();
            if (searchArguments.SearchRequired)
            {
                var properties = typeof(UserInformationDto).GetProperties().Select(p => p.Name).ToArray();
                users = users
                    .Where(_userQueryBuilder.SearchQuery<User>(searchArguments.Value!, properties));
            }

            if (searchArguments.OrderByRequired)
                users = _userQueryBuilder
                    .OrderByQuery(users, searchArguments.OrderBy!, searchArguments.OrderByType!);

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
