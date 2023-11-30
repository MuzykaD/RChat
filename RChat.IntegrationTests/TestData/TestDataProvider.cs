using RChat.Domain.Chats;
using RChat.Domain.Messages;
using RChat.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChat.IntegrationTests.TestData
{
    internal static class TestDataProvider
    {
        internal const string TestDataConnectionString = "Server=localhost\\MSSQLSERVER01;Database=RChatDb_Test;Trusted_Connection=True; TrustServerCertificate=True";
        internal static List<User> GetTestUsersList()
        {
            return new List<User>()
            {
                new()
                {
                    Id = 999,
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    Email = "admin@mail.com",
                    NormalizedEmail = "ADMIN@MAIL.COM",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAIAAYagAAAAEDwYwXeDN0pFFZnOAkQwOkuI/qd+Z4wWCr77/SzbDAAu9nB4snDnxUbruvtoPxjQeQ==",
                    SecurityStamp = "A7VKV7K7YGJBMAOV7DZ67C5SWEQU3NPZ",
                    ConcurrencyStamp = "9daa87c2-9cee-490e-bf4e-8a54dabc17b3",
                    PhoneNumber = "1234567890",
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                },
                 new()
                {
                    Id = 1,
                    UserName = "Test1",
                    NormalizedUserName = "TEST1",
                    Email = "test1@mail.com",
                    NormalizedEmail = "TEST1@MAIL.COM",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAIAAYagAAAAEKvxkiBYcN2bh1VpHPoRFfkuWUbH36j6UksDzYIJyiEf/lDuouLvlZ7tGxqP11X7Gw==",
                    SecurityStamp = "4BVGHYFTR2Z5JVHU64HAJOXFTTAFUUGM",
                    ConcurrencyStamp = "be763529-8abc-46c5-b231-1212844a141d",
                    PhoneNumber = "1234567890",
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                },
                  new()
                {
                    Id = 2,
                    UserName = "Test2",
                    NormalizedUserName = "TEST2",
                    Email = "test2@mail.com",
                    NormalizedEmail = "TEST2@MAIL.COM",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAIAAYagAAAAEJEUN2X4CPrPum8pJHdoxVRgJXl6eC8RUZuUfL9wv5xZcQ9ck+yAaDCeH+ibMc428A==",
                    SecurityStamp = "OA337V7GIP3SQVJ4F2LWVILINZGSOYRP",
                    ConcurrencyStamp = "5c3a71e8-4625-46ac-8fa3-fcde429a4461",
                    PhoneNumber = "1234567890",
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                },

            };
        }

        internal static List<Chat> GetTestChatList()
        {
            return new List<Chat>()
            {
                new()
                {
                    Id = 1,
                    IsGroupChat = false,
                    Name = "Test2-Test1",
                    CreatorId = 2,
                    Users = new List<User>() { new() {Id = 2 }, new() { Id = 1 } },
                    },
                new()
                {
                    Id = 2,
                    IsGroupChat = true,
                    Name = "AllGroupChat",
                    CreatorId = 999,
                    Users = new List<User>() { new() {Id = 999 }, new() { Id = 1 }, new() { Id = 2 } }
                }
            };
        }

        internal static List<Message> GetTestMessageList()
        {
            return new List<Message>
            {
                new() {Id = 1, Content = "MessageToDeleteByUser_CorrectSenderId_Deleted", SenderId = 999, ChatId = 2, SentAt = DateTime.Now},
                new() {Id = 2, Content = "MessageToDeleteByUser_WrongSenderId_NotDeleted", SenderId = 1, ChatId = 2, SentAt = DateTime.Now},
                new() {Id = 3, Content = "MessageToUpdateByUser", SenderId = 999, ChatId = 2, SentAt = DateTime.Now},
            };
        }
    }
}
