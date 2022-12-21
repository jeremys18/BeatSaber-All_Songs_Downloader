CREATE TABLE [dbo].[ParitySummary]
(
	[Id] INT NOT NULL PRIMARY KEY identity, 
    [errors] INT NOT NULL, 
    [warns] INT NOT NULL, 
    [resets] INT NOT NULL
)
