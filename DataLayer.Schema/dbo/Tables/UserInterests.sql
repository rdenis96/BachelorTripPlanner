CREATE TABLE [dbo].[UserInterests] (
    [UserId]             INT            NOT NULL,
    [Countries]          NVARCHAR (MAX) NULL,
    [Cities]             NVARCHAR (MAX) NULL,
    [Weathers]           BIGINT         NULL,
    [TouristAttractions] NVARCHAR (MAX) NULL,
    [Transports]         INT            NULL,
    CONSTRAINT [PK_UserInterests] PRIMARY KEY CLUSTERED ([UserId] ASC)
);

