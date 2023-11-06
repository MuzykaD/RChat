using LinqKit;
using RChat.Application.Contracts.Users;
using RChat.Domain.Users;
using System.Linq.Expressions;

namespace RChat.Application.Users
{
    public class UserQueryBuilder : IUserQueryBuilder
    {
        public Expression<Func<User, bool>> SearchQuery(string searchValues)
        {
            var words = searchValues.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var predicate = PredicateBuilder.New<User>(false);

            words.ForEach(word =>
            {
                predicate = predicate.Or(user =>
                user.UserName!.Contains(word) ||
                user.Email!.Contains(word) ||
                (user.PhoneNumber??string.Empty).Contains(word));
            });

            return predicate;
        }
    }
}
