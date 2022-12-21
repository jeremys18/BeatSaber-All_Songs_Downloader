CREATE TABLE [dbo].[Metadata](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[songName] [nvarchar](max) NOT NULL,
	[songSubName] [nvarchar](max) NOT NULL,
	[songAuthorName] [nvarchar](max) NOT NULL,
	[levelAuthorName] [nvarchar](max) NOT NULL,
	[bpm] [decimal](6, 2) NOT NULL,
	[duration] [int] NOT NULL,
 CONSTRAINT [PK_Metadata] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO