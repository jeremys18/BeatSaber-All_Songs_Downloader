CREATE TABLE [dbo].[Difficulties2](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[easyId] [int] NULL,
	[normalId] [int] NULL,
	[hardId] [int] NULL,
	[expertId] [int] NULL,
	[expertPlusId] [int] NULL,
 CONSTRAINT [PK_Difficulties2] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO