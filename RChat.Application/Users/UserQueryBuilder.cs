using LinqKit;
using RChat.Application.Contracts.Users;
using RChat.Domain.Users;
using System.Linq.Expressions;
using System.Reflection;

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
                (user.PhoneNumber ?? string.Empty).Contains(word));
            });

            return predicate;
        }

        public Expression<Func<User, TKey>> OrderByQuery<TKey>(string orderByValue, string orderByType)
        {
            Type userType = typeof(User);

            ParameterExpression param = Expression.Parameter(userType, "x");

            PropertyInfo prop = userType.GetProperty(orderByValue);

            if (prop == null)
            {
                throw new ArgumentException($"Property '{orderByValue}' not found in User class.");
            }

            Expression propAccess = Expression.Property(param, prop);
            Type delegateType = typeof(Func<,>).MakeGenericType(userType, typeof(TKey));
            LambdaExpression lambda = Expression.Lambda(delegateType, propAccess, param);

            return (Expression<Func<User, TKey>>)lambda;
        }
    }
}
