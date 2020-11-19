CREATE TABLE [dbo].[Notifications] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Type]     INT           NOT NULL,
    [UserId]   INT           NOT NULL,
    [SenderId] INT           NULL,
    [TripId]   INT           NULL,
    [Date]     DATETIME2 (7) NOT NULL,
    CONSTRAINT [PK_Notifications] PRIMARY KEY CLUSTERED ([Id] ASC)
);

