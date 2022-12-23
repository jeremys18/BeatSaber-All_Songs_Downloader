using BeatSaberSongDownloader.Data.Models.BareModels;
using MediatR;

namespace BeatSaberSongDownloader.Server.Services.MediatRServices.SongDownloader.GetAllSongs.Query
{
    public class GetAllSongsQuery : IRequest<List<Song>>
    {
        public string SongBasePath { get; set; }
    }
}
