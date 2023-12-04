CREATE TABLE [dbo].[Chats] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (MAX) NOT NULL,
    [CreatorId]   INT            NULL,
    [IsGroupChat] BIT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([CreatorId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_Chats_Chats] FOREIGN KEY ([Id]) REFERENCES [dbo].[Chats] ([Id])
);



