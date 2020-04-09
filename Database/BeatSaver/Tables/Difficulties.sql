CREATE TABLE [dbo].[Difficulties](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[easy] [bit] NOT NULL,
	[normal] [bit] NOT NULL,
	[hard] [bit] NOT NULL,
	[expert] [bit] NOT NULL,
	[expertPlus] [bit] NOT NULL,
 CONSTRAINT [PK_Difficulties] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO