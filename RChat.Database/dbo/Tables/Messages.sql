CREATE TABLE [dbo].[Messages] (
    [Id]       INT IDENTITY (1, 1) NOT NULL,
    [SenderId] INT NOT NULL,
    [ChatId]   INT NOT NULL,
    CONSTRAINT [PK_Messages] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Messages_Chats_ChatId] FOREIGN KEY ([ChatId]) REFERENCES [dbo].[Chats] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Messages_Users_SenderId] FOREIGN KEY ([SenderId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Messages_ChatId]
    ON [dbo].[Messages]([ChatId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Messages_SenderId]
    ON [dbo].[Messages]([SenderId] ASC);

