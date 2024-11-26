CREATE TABLE [dbo].[Tag](
	[Id] [int] primary key IDENTITY(1,1) NOT NULL,
	[SongId] int not null,
	[Name] nvarchar(500) not null,

	constraint FK_Tag_Song foreign key (SongId) references dbo.Song(SongId)
)
GO