
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.IntegrationTests.DbSqlHelpers
{
    internal class DbSqlHelper
    {
        protected const string _connectionString = "Server=localhost\\MSSQLSERVER01;Database=RChatDb_Test;Trusted_Connection=True; TrustServerCertificate=True";
        protected SqlDbService sqlDbService;
        public DbSqlHelper()
        {
            sqlDbService = new(_connectionString);
        }

        public async Task SetTableDirectIdInsertToOn(string tableName)
        {
            var query = $"SET IDENTITY_INSERT [dbo].[{tableName}] ON";
            await sqlDbService.ExecuteQueryAsync(query);
        }
        public async Task SetTableDirectIdInsertToOff(string tableName)
        {
            var query = $"SET IDENTITY_INSERT [dbo].[{tableName}] OFF";
            await sqlDbService.ExecuteQueryAsync(query);
        }
    }
}
