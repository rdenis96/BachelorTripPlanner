CREATE TABLE [dbo].[Users] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Email]        NVARCHAR (100) NOT NULL,
    [Password]     NVARCHAR (MAX) NOT NULL,
    [RegisterDate] DATETIME2 (7)  NOT NULL,
    [LastOnline]   DATETIME2 (7)  NULL,
    [Ip]           NVARCHAR (50)  NOT NULL,
    [Phone]        NVARCHAR (15)  NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);

