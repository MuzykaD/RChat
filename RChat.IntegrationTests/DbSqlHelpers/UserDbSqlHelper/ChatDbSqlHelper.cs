using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.IntegrationTests.DbSqlHelpers.UserDbSqlHelper
{
    internal class ChatDbSqlHelper : DbSqlHelper
    {
        public async Task DeleteTestChatDataAsync(string chatName)
        {
            var query = "DELETE FROM [dbo].[Chats] WHERE NAME = @name";
            var queryParametes = new Dictionary<string, object>
            {
                {"@name", chatName }
            };
           await sqlDbService.ExecuteQueryAsync(query, queryParametes);  
        }
    }
}
