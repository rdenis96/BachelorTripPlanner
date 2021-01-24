CREATE TABLE [dbo].[Friends] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [UserId]      INT           NOT NULL,
    [FriendId]    INT           NOT NULL,
    [CreatedDate] DATETIME2 (7) NOT NULL,
    [IsDeleted]   BIT           CONSTRAINT [DF_Friends_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Friends] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_FriendId_Friends_Users] FOREIGN KEY ([FriendId]) REFERENCES [dbo].[Users] ([Id]),
    CONSTRAINT [FK_UserId_Friends_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);



