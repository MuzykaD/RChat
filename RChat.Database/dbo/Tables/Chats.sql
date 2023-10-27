CREATE TABLE [dbo].[Chats] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (MAX) NOT NULL,
    [CreatorId] INT            NULL,
    CONSTRAINT [PK_Chats] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Chats_Users_CreatorId] FOREIGN KEY ([CreatorId]) REFERENCES [dbo].[Users] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Chats_CreatorId]
    ON [dbo].[Chats]([CreatorId] ASC);

