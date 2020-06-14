CREATE TABLE [dbo].[Interests] (
    [Id]                 INT            IDENTITY (1, 1) NOT NULL,
    [Country]            NVARCHAR (MAX) NOT NULL,
    [City]               NVARCHAR (MAX) NOT NULL,
    [GeneralWeather]     NVARCHAR (50)  NOT NULL,
    [Weather]            NVARCHAR (MAX) NOT NULL,
    [TouristAttractions] NVARCHAR (MAX) NOT NULL,
    [Transport]          NVARCHAR (MAX) NOT NULL,
    [LinkImage]          NVARCHAR (MAX) NULL,
    [LinkWikipediaCity]  NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Interests] PRIMARY KEY CLUSTERED ([Id] ASC)
);

