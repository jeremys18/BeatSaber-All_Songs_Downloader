CREATE TABLE [dbo].[DifficultyInfo](
	[Id] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[njs] FLOAT NOT NULL,
	[offset] FLOAT(25) NOT NULL,
	[notes] [int] NOT NULL,
	[bombs] [int] not null,
	[obstacles] [int] NOT NULL,
	[nps] FLOAT NOT NULL,
	[length] FLOAT NOT NULL,
	[characteristic] NVARCHAR(250) NOT NULL, 
    [difficulty] NVARCHAR(100) NOT NULL, 
    [events] INT NOT NULL, 
    [chroma] INT NOT NULL, 
    [me] BIT NOT NULL, 
    [ne] BIT NOT NULL, 
    [cinema] BIT NOT NULL, 
    [seconds] FLOAT NOT NULL, 
    [maxScore] INT NOT NULL,
	[label] nvarchar(max),
	[paritySummaryId] INT NOT NULL,
	[environment] nvarchar(100) null,
	[versionid] int not null,
	
	constraint diff_parity foreign key (paritySummaryId) references dbo.ParitySummary(Id),
	constraint diff_version foreign key (versionId) references dbo.[Version](Id)
	)
GO