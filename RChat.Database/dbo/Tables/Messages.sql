CREATE TABLE [dbo].[Messages] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [SenderId] INT            NOT NULL,
    [ChatId]   INT            NOT NULL,
    [Content]  NVARCHAR (450) NOT NULL,
    [SentAt]   DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([SenderId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK__Messages__ChatId__403A8C7D] FOREIGN KEY ([ChatId]) REFERENCES [dbo].[Chats] ([Id]) ON DELETE CASCADE
);



