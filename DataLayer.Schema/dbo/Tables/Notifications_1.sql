CREATE TABLE [dbo].[Notifications] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Type]     INT           NOT NULL,
    [UserId]   INT           NOT NULL,
    [SenderId] INT           NULL,
    [TripId]   INT           NULL,
    [Date]     DATETIME2 (7) NOT NULL,
    CONSTRAINT [PK_Notifications] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SenderId_Notifications_Users] FOREIGN KEY ([SenderId]) REFERENCES [dbo].[Users] ([Id]),
    CONSTRAINT [FK_TripId_Notifications_Trips] FOREIGN KEY ([TripId]) REFERENCES [dbo].[Trips] ([Id]),
    CONSTRAINT [FK_UserId_Notifications_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);



