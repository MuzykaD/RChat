CREATE TABLE [dbo].[Attachments] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [MessageId] INT            NOT NULL,
    [FileName]  NVARCHAR (MAX) NOT NULL,
    [FileUrl]   NVARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([MessageId]) REFERENCES [dbo].[Messages] ([Id])
);

