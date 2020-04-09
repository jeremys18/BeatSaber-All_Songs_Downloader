CREATE TABLE [dbo].[DifficultyInfo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[duration] [decimal](9, 4) NOT NULL,
	[length] [int] NOT NULL,
	[bombs] [int] NOT NULL,
	[notes] [int] NOT NULL,
	[obstacles] [int] NOT NULL,
	[njs] [decimal](18, 6) NOT NULL,
	[njsOffset] [decimal](18, 17) NOT NULL,
 CONSTRAINT [PK_DifficultyInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO