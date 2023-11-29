using RChat.Domain.Users;
using RChat.Domain.Users.DTO;
using RChat.IntegrationTests.Configuration;
using RChat.IntegrationTests.DbSqlHelpers;
using RChat.IntegrationTests.SqlScripts;
using RChat.IntegrationTests.TestData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace RChat.IntegrationTests
{
    public class TestBase
    {
        protected TestApplicationFactory factory;
        private SqlDbService _sqlService;
        private const string _connectionString = "Server=localhost\\MSSQLSERVER01;Database=RChatDb_Test;Trusted_Connection=True; TrustServerCertificate=True";
        public TestBase()
        {
            factory = new();
            _sqlService = new(_connectionString);
        }

        protected async Task SeedUsersDataAsync()
        {
            var users = TestDataProvider.GetTestUsersList();
            var parameters = new Dictionary<string, object>();
            for (int i = 0; i < users.Count; i++)
            {
                parameters.Add($"@id{i}", users[i].Id);
                parameters.Add($"@userName{i}", users[i].UserName!);
                parameters.Add($"@normUserName{i}", users[i].NormalizedUserName!);
                parameters.Add($"@email{i}", users[i].Email!);
                parameters.Add($"@normEmail{i}", users[i].NormalizedEmail!);
                parameters.Add($"@emailConfirm{i}", users[i].EmailConfirmed);
                parameters.Add($"@password{i}", users[i].PasswordHash!);
                parameters.Add($"@security{i}", users[i].SecurityStamp!);
                parameters.Add($"@concurrency{i}", users[i].ConcurrencyStamp);
                parameters.Add($"@phone{i}", users[i].PhoneNumber);
                parameters.Add($"@phoneConfirmed{i}", users[i].PhoneNumberConfirmed);
                parameters.Add($"@twoFactor{i}", users[i].TwoFactorEnabled);
                parameters.Add($"@lock{i}", users[i].LockoutEnabled);
                parameters.Add($"@accessCount{i}", users[i].AccessFailedCount);
            }
            var query = SqlScripts.SqlScripts.SeedUsersScript(users.Count());
            await _sqlService.ExecuteQueryAsync(query, parameters);
        }
        protected async Task SeedChatsDataAsync()
        {
            var chats = TestDataProvider.GetTestChatList();
            var parameters = new Dictionary<string, object>();
            for (int i = 0; i < chats.Count; i++)
            {
                parameters.Add($"@id{i}", chats[i].Id);
                parameters.Add($"@name{i}", chats[i].Name!);
                parameters.Add($"@creatorId{i}", chats[i].CreatorId!);
                parameters.Add($"@isGroupChat{i}", chats[i].IsGroupChat!);
            }
            var query = SqlScripts.SqlScripts.SeedChatsScript(chats.Count());
            await _sqlService.ExecuteQueryAsync(query,parameters);
        }
        protected async Task SeedChatUsersDataAsync()
        {
            var chats = TestDataProvider.GetTestChatList();
            var parameters = new Dictionary<string, object>();
            foreach(var chat in chats)
            {
                for (int i = 0; i < chat.Users.Count(); i++)
                {
                    var userId = chat.Users.ElementAt(i).Id;
                    parameters.Add($"@chat{chat.Id}{i}", chat.Id);
                    parameters.Add($"@user{chat.Id}{userId}{i}", userId);
                }
            }
            var query = SqlScripts.SqlScripts.SeedChatUsersScript(chats);
            await _sqlService.ExecuteQueryAsync(query, parameters);
        }
        protected async Task SeedMessagesDataAsync()
        {
            var messages = TestDataProvider.GetTestMessageList();
            var parameters = new Dictionary<string, object>();
            for (int i = 0; i < messages.Count(); i++)
            {
                parameters.Add($"id{i}", messages[i].Id);
                parameters.Add($"senderId{i}", messages[i].SenderId);
                parameters.Add($"chatId{i}", messages[i].ChatId);
                parameters.Add($"content{i}", messages[i].Content);
                parameters.Add($"sentAt{i}", messages[i].SentAt);
            }
            var query = SqlScripts.SqlScripts.SeedMessagesScripte(messages.Count());
            await _sqlService.ExecuteQueryAsync(query,parameters);
        }

        protected async Task ClearTables()
        {
            await _sqlService.ExecuteQueryAsync(SqlScripts.SqlScripts.ClearAllTablesScript());
        }


    }
}
