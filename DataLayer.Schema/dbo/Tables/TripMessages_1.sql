CREATE TABLE [dbo].[TripMessages] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [SenderId] INT            NOT NULL,
    [TripId]   INT            NOT NULL,
    [Text]     NVARCHAR (500) NOT NULL,
    [Date]     DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_TripMessages] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SenderId_TripMessages_Users] FOREIGN KEY ([SenderId]) REFERENCES [dbo].[Users] ([Id]),
    CONSTRAINT [FK_TripId_TripMessages_Trips] FOREIGN KEY ([TripId]) REFERENCES [dbo].[Trips] ([Id])
);



