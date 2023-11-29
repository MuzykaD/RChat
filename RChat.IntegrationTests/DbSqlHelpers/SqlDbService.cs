using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.IntegrationTests.DbSqlHelpers
{
    internal class SqlDbService
    {
        private readonly string _connectionString;
        private SqlConnection GetSqlConnection => new(_connectionString);
        public SqlDbService(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task ExecuteQueryAsync(string query)
        {
            using (var connection = GetSqlConnection)
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = query;
                await command.ExecuteNonQueryAsync();
            }
        }
        public async Task ExecuteQueryAsync(string query, Dictionary<string, object> parameters)
        {
            using (var connection = GetSqlConnection)
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddRange(parameters.Select(kvp => new SqlParameter(kvp.Key, kvp.Value)).ToArray());
                await command.ExecuteNonQueryAsync();
            }
        }
        public async Task<T> ExecuteScalarAsync<T>(string query, Dictionary<string, object> parameters)
        {
            using (var connection = GetSqlConnection)
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddRange(parameters.Select(kvp => new SqlParameter(kvp.Key, kvp.Value)).ToArray());
               return (T)await command.ExecuteScalarAsync();
            }
        }

    }
}
