CREATE TABLE [dbo].[Version]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [SongId] INT NOT NULL, 
    [hash] NVARCHAR(100) NOT NULL, 
    [state] NVARCHAR(100) NOT NULL, 
    [createdAt] DATETIME NOT NULL, 
    [sageScore] DECIMAL(18, 4) NOT NULL, 
    [downloadURL] NVARCHAR(250) NOT NULL, 
    [coverURL] NVARCHAR(250) NOT NULL, 
    [previewURL] NVARCHAR(250) NOT NULL,

    constraint version_song foreign key (SongId) references dbo.Song([SongId])
)
