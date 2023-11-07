using Microsoft.AspNetCore.Identity;
using RChat.Application.Contracts.Common;
using RChat.Application.Contracts.Users;
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
        public async Task<GridListDto<UserInformationDto>> GetUsersInformationListAsync(string? value, int skip = 0, int takeCount = 5, string? orderBy = null, string? orderByType = null)
        {
            var users = _userManager.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(value))
                users = users.Where(_userQueryBuilder.SearchQuery<User>(value, "UserName","Email","PhoneNumber"));

            if (!string.IsNullOrWhiteSpace(orderBy) && !string.IsNullOrWhiteSpace(orderByType))
                users = _userQueryBuilder.OrderByQuery(users, orderBy, orderByType);
            
            var totalCount = users.Count();
            
            var userInformation = users.Skip(skip).Take(takeCount).Select(u => new UserInformationDto()
            {
                Email = u.Email,
                UserName = u.UserName,
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
