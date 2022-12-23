using BeatSaberDownloader.Data.Repositories;
using BeatSaberSongDownloader.Data.Models.BareModels;
using BeatSaberSongDownloader.Server.Handlers;
using MediatR;

namespace BeatSaberSongDownloader.Server.Services.MediatRServices.SongDownloader.GetAllSongs.Query
{
    public class GetAllSongsQueryHandler : IRequestHandler<GetAllSongsQuery, List<Song>>
    {
        private IConfiguration _configuration;
        public GetAllSongsQueryHandler(IConfiguration configuration)
        {
            _configuration= configuration;
        }

        public async Task<List<Song>> Handle(GetAllSongsQuery query, CancellationToken cancellationToken)
        {
           
            var result = new List<Song>();

            // Get all songs info
            var songInfo = new BeatSaverRepository(_configuration).GetAllSongs();

            //Convert to basic model
            foreach (var song in songInfo)
            {
                var verNumber = 1;
                foreach (var version in song.versions)
                {
                    var songBasic = new Song
                    {
                        Id = song.id,
                        Name = song.name,
                        BeatSaverDownloadUrl = version.downloadURL,
                        FileName = TextHandler.GetValidFileName(query.SongBasePath, song, verNumber) // May change in future but for now just leave without base path
                    };

                    result.Add(songBasic);
                    verNumber++; // Because there can be more than 1 ver of the same file we need to differ between them
                }
            }

            return result;
        }
    }
}
