using RChat.Domain.Users.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.IntegrationTests.DbSqlHelpers.UserDbSqlHelper
{
    internal class UserDbSqlHelper : DbSqlHelper
    {
        public async Task RemoveTestAuthenticationUsersAsync(string name)
        {
            var query = """
                        DELETE FROM [dbo].[AspNetUsers]
                        WHERE [UserName] = @name
                        """;
            var parametersDict = new Dictionary<string, object>
            { {"@name", name } };
            await sqlDbService.ExecuteQueryAsync(query, parametersDict);
        }

        public async Task InsertTestUserAsync(RegisterUserDto registerDto)
        {
            var query = """
                        INSERT INTO [dbo].[AspNetUsers] ([UserName],[NormalizedUserName], [Email], [NormalizedEmail] , [EmailConfirmed], 
                        [PasswordHash], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount])
                        VALUES
                        (@username,@normUsername, @email, @normEmail, @emailConfirmed, @password, @phoneConfirmed, @twoFactor, @lockout, @accessCount)
                        """;
            var parametersDict = new Dictionary<string, object>
            {
                {"@username", registerDto.Username},
                {"@normUsername", registerDto.Username.ToUpper()},
                {"@email", registerDto.Email },
                {"@normEmail", registerDto.Email.ToUpper() },
                {"@emailConfirmed", false},
                {"@password", registerDto.Password },
                {"@phoneConfirmed", false },
                {"@twoFactor", false },
                {"@lockout", false },
                {"@accessCount", 0},
            };
            await sqlDbService.ExecuteQueryAsync(query, parametersDict);
        }
    }
}
