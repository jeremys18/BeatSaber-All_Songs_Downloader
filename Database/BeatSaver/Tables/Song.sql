CREATE TABLE [dbo].[Song](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[_id] [nvarchar](128) NOT NULL,
	[key] [nvarchar](128) NOT NULL,
	[hash] [nvarchar](128) NOT NULL,
	[name] [nvarchar](max) NOT NULL,
	[description] [nvarchar](max) NOT NULL,
	[directDownload] [nvarchar](max) NOT NULL,
	[downloadURL] [nvarchar](max) NOT NULL,
	[coverURL] [nvarchar](max) NOT NULL,
	[uploaded] [nvarchar](max) NOT NULL,
	[deletedAt] [nvarchar](128) NULL,
	[metadataId] [int] NOT NULL,
	[statsId] [int] NOT NULL,
	[uploaderId] [int] NOT NULL,
 CONSTRAINT [PK_Song] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO