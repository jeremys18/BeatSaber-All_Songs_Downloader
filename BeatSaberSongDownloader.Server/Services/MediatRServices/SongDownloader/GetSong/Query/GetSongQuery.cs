using MediatR;

namespace BeatSaberSongDownloader.Server.Services.MediatRServices.SongDownloader.GetSong.Query
{
    public class GetSongQuery : IRequest<Byte[]>
    {
        public string SongId { get; set; }
        public string VersionHash { get; set;}
    }
}
