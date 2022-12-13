CREATE TABLE [dbo].[DifficultyInfo](
	[Id] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[njs] DECIMAL(18, 6) NOT NULL,
	[offset] DECIMAL(18, 6) NOT NULL,
	[notes] [int] NOT NULL,
	[bombs] [int] not null,
	[obstacles] [int] NOT NULL,
	[nps] [decimal](18, 6) NOT NULL,
	[length] [decimal](18, 17) NOT NULL,
	[characteristic] NVARCHAR(250) NOT NULL, 
    [difficulty] NVARCHAR(100) NOT NULL, 
    [events] INT NOT NULL, 
    [chroma] INT NOT NULL, 
    [me] BIT NOT NULL, 
    [ne] BIT NOT NULL, 
    [cinema] BIT NOT NULL, 
    [seconds] DECIMAL(18, 6) NOT NULL, 
    [maxScore] INT NOT NULL,
	[paritySummaryId] INT NOT NULL,
	[versionid] int not null,
	
	constraint diff_parity foreign key (paritySummaryId) references dbo.ParitySummary(Id),
	constraint diff_version foreign key (versionId) references dbo.[Version](Id)
	)
GO