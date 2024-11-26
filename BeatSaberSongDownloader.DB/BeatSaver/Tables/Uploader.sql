CREATE TABLE [dbo].[Uploader](
	[UploaderId] [int] primary key IDENTITY(1,1) NOT NULL,
	[id] [nvarchar](128) NOT NULL,
	[name] [nvarchar](max) NOT NULL,
	[avatar] NVARCHAR(250) NOT NULL, 
    [type] NVARCHAR(250) NOT NULL, 
    [admin] BIT NOT NULL, 
    [curator] bit NOT NULL,
	[seniorCurator] bit not null,
	[playlistUrl] nvarchar(500)
)
GO