CREATE TABLE [dbo].[ChatUser] (
    [ChatsId] INT NOT NULL,
    [UsersId] INT NOT NULL,
    CONSTRAINT [PK_ChatUser] PRIMARY KEY CLUSTERED ([ChatsId] ASC, [UsersId] ASC),
    FOREIGN KEY ([UsersId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK__ChatUser__ChatsI__3C69FB99] FOREIGN KEY ([ChatsId]) REFERENCES [dbo].[Chats] ([Id]) ON DELETE CASCADE
);



