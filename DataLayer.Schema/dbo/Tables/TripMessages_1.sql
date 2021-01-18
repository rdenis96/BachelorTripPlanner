CREATE TABLE [dbo].[TripMessages] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [SenderId] INT            NOT NULL,
    [Tripid]   INT            NOT NULL,
    [Text]     NVARCHAR (500) NOT NULL,
    [Date]     DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_TripMessages] PRIMARY KEY CLUSTERED ([Id] ASC)
);

