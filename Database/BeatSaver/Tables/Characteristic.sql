CREATE TABLE [dbo].[Characteristic](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](max) NOT NULL,
	[Difficulties2Id] [int] NOT NULL,
	[MetadataId] [int] NOT NULL,
	CONSTRAINT [PK_Characteristic] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	CONSTRAINT [FK_Characteristic_Difficulties2] FOREIGN KEY([Difficulties2Id]) REFERENCES [dbo].[Difficulties2] ([Id])
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO