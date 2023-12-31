﻿namespace RChat.UI.Common
{
    public static class RChatApiRoutes
    {
        public const string Login = "/api/v1/authentication/login";
        public const string Register = "/api/v1/authentication/register";
        public const string ChangePassword = "/api/v1/account/change-password";
        public const string Info = "/api/v1/account/profile";
        public const string SignalGroupsInfo = "/api/v1/chats/group-info";
        public const string UpdateInfo = "/api/v1/account/update-profile";
        public const string Users = "/api/v1/users";
        public const string Chats = "/api/v1/chats";
        public const string ChatsPrivate = "/api/v1/chats/private";
        public const string ChatsGroup = "/api/v1/chats/group";
        public const string Messages = "/api/v1/messages";
    }
}
