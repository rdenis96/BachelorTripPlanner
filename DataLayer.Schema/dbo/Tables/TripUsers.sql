CREATE TABLE [dbo].[TripUsers] (
    [Id]                    INT IDENTITY (1, 1) NOT NULL,
    [UserId]                INT NOT NULL,
    [TripId]                INT NOT NULL,
    [HasAcceptedInvitation] BIT CONSTRAINT [DF_TripUser_HasAcceptedInvitation] DEFAULT ((0)) NOT NULL,
    [IsGroupAdmin]          BIT NOT NULL,
    [IsDeleted]             BIT CONSTRAINT [DF_TripUsers_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_TripUser] PRIMARY KEY CLUSTERED ([Id] ASC)
);



