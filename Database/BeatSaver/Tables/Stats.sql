CREATE TABLE [dbo].[Stats](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[downloads] [int] NOT NULL,
	[plays] [int] NOT NULL,
	[downVotes] [int] NOT NULL,
	[upVotes] [int] NOT NULL,
	[heat] [numeric](18, 7) NOT NULL,
	[rating] [decimal](18, 17) NOT NULL,
 CONSTRAINT [PK_Stats] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO