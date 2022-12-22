using BeatSaberDownloader.Data.DBContext;
using BeatSaberSongDownloader.Data.Models.DetailedModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BeatSaberDownloader.Data.Repositories
{
    public class BeatSaverRepository 
    {
        private bool isDisposed;
        private BeatSaverContext _context;

        public BeatSaverRepository(IConfiguration configuration)
        {
            _context = new BeatSaverContext(configuration);
        }

        public List<Song> GetAllSongs()
        {
            var result = new List<Song>();

            using (var context = _context)
            {
                result = _context.Songs
                    .Include("Metadata")
                    .Include("Uploader")
                    .Include("Stats")
                    .Include("Versions.Diffs.ParitySummary")
                    .ToList();
            }

            return result;
        }

        public void SaveSongsToDb(List<Song> songs)
        {
            if (songs == null) return;

            using (var context = _context)
            {
                var loopCount = songs.Count < 1000 ? 1 : (songs.Count / 1000) + 1;
                for (int i = 0; i < loopCount; i++)
                {
                    try
                    {
                        var currentSongs = songs.Skip(1000 * i).Take(1000).ToList();
                        UpsertSongs(currentSongs, context);
                        context.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        var f = e.Message;
                    }
                }
            }
        }

        public void UpsertSongs(List<Song> songs, BeatSaverContext context)
        {
            foreach(var song in songs)
            {
                var dbSong = context.Songs.FirstOrDefault(x => x.id == song.id);
                if(song != null)
                {
                    // Update
                }
                else
                {
                    // ensure the uploader is correct as uploaders can upload many songs
                    var dbUploader = context.Uploaders.FirstOrDefault(x => x.id == song.uploader.id);
                    song.uploader = dbUploader ?? song.uploader;
                    context.Songs.Add(song);
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed) return;

            if (disposing)
            {
                _context.Dispose();
            }

            isDisposed = true;
        }
    }
}
