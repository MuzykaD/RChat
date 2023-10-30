CREATE TABLE [dbo].[ChatUser] (
    [ChatsId] INT NOT NULL,
    [UsersId] INT NOT NULL,
    CONSTRAINT [PK_ChatUser] PRIMARY KEY CLUSTERED ([ChatsId] ASC, [UsersId] ASC),
    FOREIGN KEY ([ChatsId]) REFERENCES [dbo].[Chats] ([Id]),
    FOREIGN KEY ([UsersId]) REFERENCES [dbo].[Users] ([Id])
);

