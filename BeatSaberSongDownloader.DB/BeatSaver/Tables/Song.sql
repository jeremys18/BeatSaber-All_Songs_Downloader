CREATE TABLE [dbo].[Song](
	[SongId] [int] IDENTITY(1,1) NOT NULL,
	[id] [nvarchar](128) NOT NULL,
	[name] [nvarchar](max) NOT NULL,
	[description] [nvarchar](max) NOT NULL,
	[uploaded] datetime2(7) NOT NULL,
	[automapper] [bit] not null,
	[ranked] [bit] not null,
	[qualified] bit not null,
	[createdAt] datetime2(7) NOT NULL,
	[updatedAt] datetime2(7) NOT NULL,
	[lastPublishedAt] datetime2(7) NOT NULL,
	[declaredAi] [nvarchar](125) not null,
	[blRanked] [bit] not null,
	[blQualified] [bit] not null,
	[bookmarked] bit not null, 
	[metadataId] [int] NOT NULL,
	[statsId] [int] NOT NULL,
	[uploaderId] [int] NOT NULL,

	constraint song_uploader foreign key(uploaderId) references dbo.Uploader(UploaderId),
	constraint song_meta foreign key(metadataId) references dbo.Metadata(Id),
	constraint song_stats foreign key(statsId) references dbo.[Stats](Id),

	CONSTRAINT [PK_Song] PRIMARY KEY CLUSTERED ([SongId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO