CREATE TABLE [dbo].[Trips] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (250) NOT NULL,
    [Type] INT            CONSTRAINT [DF_Trip_Type] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Trip] PRIMARY KEY CLUSTERED ([Id] ASC)
);



