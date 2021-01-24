CREATE TABLE [dbo].[SimilarInterests] (
    [Id]            INT IDENTITY (1, 1) NOT NULL,
    [InterestId]    INT NOT NULL,
    [SimInterestId] INT NOT NULL,
    CONSTRAINT [PK_SimilarInterests] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_InterestId_SimilarInterests_Interests] FOREIGN KEY ([InterestId]) REFERENCES [dbo].[Interests] ([Id]),
    CONSTRAINT [FK_SimilarInterestId_SimilarInterests_Interests] FOREIGN KEY ([SimInterestId]) REFERENCES [dbo].[Interests] ([Id])
);

