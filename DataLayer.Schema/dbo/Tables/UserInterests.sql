CREATE TABLE [dbo].[UserInterests] (
    [Id]                 INT            IDENTITY (1, 1) NOT NULL,
    [UserId]             INT            NOT NULL,
    [TripId]             INT            NULL,
    [Countries]          NVARCHAR (MAX) NOT NULL,
    [Cities]             NVARCHAR (MAX) NULL,
    [TouristAttractions] NVARCHAR (MAX) NULL,
    [Transports]         NVARCHAR (MAX) CONSTRAINT [DF__UserInter__Trans__24927208] DEFAULT ((0)) NOT NULL,
    [Weather]            NVARCHAR (MAX) CONSTRAINT [DF__UserInter__Weath__239E4DCF] DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TripId] FOREIGN KEY ([TripId]) REFERENCES [dbo].[Trips] ([Id]),
    CONSTRAINT [FK_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);







