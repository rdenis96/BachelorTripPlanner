CREATE TABLE [dbo].[UserInterests] (
    [UserId]             INT            NOT NULL,
    [Countries]          NVARCHAR (MAX) NOT NULL,
    [Cities]             NVARCHAR (MAX) NULL,
    [TouristAttractions] NVARCHAR (MAX) NULL,
    [Transports]         NVARCHAR (MAX) CONSTRAINT [DF__UserInter__Trans__24927208] DEFAULT ((0)) NOT NULL,
    [Weather]            NVARCHAR (MAX) CONSTRAINT [DF__UserInter__Weath__239E4DCF] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_UserInterests] PRIMARY KEY CLUSTERED ([UserId] ASC)
);



