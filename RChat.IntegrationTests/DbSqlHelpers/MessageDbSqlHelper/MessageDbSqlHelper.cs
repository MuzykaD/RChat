using RChat.Domain.Messages.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.IntegrationTests.DbSqlHelpers.MessageDbSqlHelper
{
    internal class MessageDbSqlHelper : DbSqlHelper
    {
        public async Task InsertTestMessageRangeAsync(List<MessageInformationDto> messageInformationDtos)
        {
            var queryStringBuilder = new StringBuilder();
            queryStringBuilder.Append("INSERT INTO [dbo].[Messages] ([Id], [SenderId], [ChatId], [Content], [SentAt]) VALUES ");
            var parametersDict = new Dictionary<string, object>();
            for (int i = 0; i < messageInformationDtos.Count(); i++)
            {
                queryStringBuilder.Append($"(@id{i}, @senderId{i}, @chatId{i}, @content{i}, @sentAt{i}),");
                parametersDict.Add($"@id{i}", messageInformationDtos[i].Id);
                parametersDict.Add($"@senderId{i}", messageInformationDtos[i].SenderId);
                parametersDict.Add($"@chatId{i}", messageInformationDtos[i].ChatId);
                parametersDict.Add($"@content{i}", messageInformationDtos[i].Content);
                parametersDict.Add($"@sentAt{i}", messageInformationDtos[i].SentAt);
            }
            queryStringBuilder.Length--;
            await SetTableDirectIdInsertToOn("Messages");
            await sqlDbService.ExecuteQueryAsync(queryStringBuilder.ToString(), parametersDict);
            await SetTableDirectIdInsertToOff("Messages");
        }

        public async Task<int> InsertMessageAndGetIdAsync(MessageInformationDto message)
        {
            var query = """
                        INSERT INTO [dbo].[Messages] ([SenderId], [ChatId], [Content], [SentAt]) OUTPUT INSERTED.Id VALUES
                        (@senderId, @chatId, @content, @sentAt)
                        """;
            var parametersDict = new Dictionary<string, object>()
            {
                {"@senderId", message.SenderId},
                {"@chatId", message.ChatId},
                {"@content", message.Content},
                {"@sentAt", message.SentAt},
            };
            return await sqlDbService.ExecuteScalarAsync<int>(query, parametersDict);
        }

        public async Task RemoveTestMessageByIdAsync(int id)
        {
            var query = "DELETE FROM [dbo].[Messages] WHERE [Id] = @id";
            var parametersDict = new Dictionary<string, object>()
            {
                {"@id", id},
            };
            await sqlDbService.ExecuteQueryAsync(query, parametersDict);
        }
    }
}
