CREATE TABLE [dbo].[ChatUser] (
    [ChatsId] INT NOT NULL,
    [UsersId] INT NOT NULL,
    CONSTRAINT [PK_ChatUser] PRIMARY KEY CLUSTERED ([ChatsId] ASC, [UsersId] ASC),
    CONSTRAINT [FK_ChatUser_Chats_ChatsId] FOREIGN KEY ([ChatsId]) REFERENCES [dbo].[Chats] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ChatUser_Users_UsersId] FOREIGN KEY ([UsersId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ChatUser_UsersId]
    ON [dbo].[ChatUser]([UsersId] ASC);

