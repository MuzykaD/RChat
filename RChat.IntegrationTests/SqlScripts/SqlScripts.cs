using Microsoft.AspNetCore.Http;
using RChat.Domain.Chats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RChat.IntegrationTests.SqlScripts
{
    public static class SqlScripts
    {
        public static string SeedUsersScript(int usersCount)
        {
            var paramsBuilder = new StringBuilder();
            for (int i = 0; i < usersCount; i++)
            {
                paramsBuilder.Append($"(@id{i}, @userName{i}, @normUserName{i}, @email{i}, @normEmail{i}, @emailConfirm{i}, @password{i}, @security{i}, @concurrency{i}, @phone{i}, @phoneConfirmed{i}, @twoFactor{i}, @lock{i}, @accessCount{i}),");
            }
            paramsBuilder.Length--;
            return $"""
                   SET IDENTITY_INSERT [dbo].[AspNetUsers] ON 
                   INSERT INTO [dbo].[AspNetUsers] ([Id]
                   ,[UserName]
                   ,[NormalizedUserName]
                   ,[Email]
                   ,[NormalizedEmail]
                   ,[EmailConfirmed]
                   ,[PasswordHash]
                   ,[SecurityStamp]
                   ,[ConcurrencyStamp]
                   ,[PhoneNumber]
                   ,[PhoneNumberConfirmed]
                   ,[TwoFactorEnabled]
                   ,[LockoutEnabled]
                   ,[AccessFailedCount] )
                   VALUES
                   {paramsBuilder.ToString()}
                   SET IDENTITY_INSERT [dbo].[AspNetUsers] OFF
                   """;
        }

        public static string SeedChatsScript(int chatCount) 
        { 
            var paramsBuilder = new StringBuilder();
            for (int i = 0; i < chatCount; i++)
            {
                paramsBuilder.Append($"(@id{i}, @name{i}, @creatorId{i}, @isGroupChat{i}),");
            }
            paramsBuilder.Length--;
            return $"""
                    SET IDENTITY_INSERT [dbo].[Chats] ON
                    INSERT INTO [dbo].[Chats]
                    ([Id]
                    ,[Name]
                    ,[CreatorId]
                    ,[IsGroupChat])
                    VALUES
                    {paramsBuilder.ToString()}
                    SET IDENTITY_INSERT [dbo].[Chats] OFF
                    """;
        }

        public static string SeedChatUsersScript(List<Chat> chats)
        {
            var paramsBuilder = new StringBuilder();
            foreach (var chat in chats)
            {
                for (int i = 0; i < chat.Users.Count(); i++)
                {
                    var userId = chat.Users.ElementAt(i).Id;
                    paramsBuilder.Append($"(@chat{chat.Id}{i}, @user{chat.Id}{userId}{i}),");
                }
            }
            paramsBuilder.Length--;
            return $"""
                    INSERT INTO [dbo].[ChatUser]
                    ([ChatsId]
                    ,[UsersId])
                    VALUES
                    {paramsBuilder.ToString()}
                    """;
        }

        public static string SeedMessagesScripte(int messagesCount)
        {
            var paramsBuilder = new StringBuilder();
            for (int i = 0; i < messagesCount; i++)
            {
                paramsBuilder.Append($"(@id{i}, @senderId{i}, @chatId{i}, @content{i}, @sentAt{i}),");
            }
            paramsBuilder.Length--;
            return $"""
                    SET IDENTITY_INSERT [dbo].[Messages] ON
                    INSERT INTO [dbo].[Messages]
                    ([Id]
                    ,[SenderId]
                    ,[ChatId]
                    ,[Content]
                    ,[SentAt])
                    VALUES
                    {paramsBuilder.ToString()}
                    SET IDENTITY_INSERT [dbo].[Messages] OFF
                    """;
        }

        public static string ClearAllTablesScript()
        {
            return """
                   DELETE FROM [dbo].[Attachments]
                   DELETE FROM [dbo].[Messages]
                   DELETE FROM [dbo].[ChatUser]
                   DELETE FROM [dbo].[Chats]
                   DELETE FROM [dbo].[AspNetUsers]
                   """;
        }
        public static string ClearSpecificTableScript(string tableName)
        {
            return $"""
                   DELETE FROM [dbo].[{tableName}]
                   """;
        }
    }
}
