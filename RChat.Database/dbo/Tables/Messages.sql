CREATE TABLE [dbo].[Messages] (
    [Id]       INT IDENTITY (1, 1) NOT NULL,
    [SenderId] INT NOT NULL,
    [ChatId]   INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([ChatId]) REFERENCES [dbo].[Chats] ([Id]),
    FOREIGN KEY ([SenderId]) REFERENCES [dbo].[Users] ([Id])
);

