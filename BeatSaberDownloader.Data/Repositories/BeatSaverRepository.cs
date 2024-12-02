using BeatSaberDownloader.Data.DBContext;
using BeatSaberDownloader.Data.Models.DetailedModels;
using BeatSaberSongDownloader.Data.Models.DetailedModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BeatSaberDownloader.Data.Repositories
{
    public class BeatSaverRepository 
    {
        private bool isDisposed;
        private BeatSaverContext _context;
        private readonly ILogger _logger;

        public BeatSaverRepository(IConfiguration configuration, ILogger<StupidLogger> logger)
        {
            _context = new BeatSaverContext(configuration);
            _logger = logger;
        }

        public Song GetSongById(string id)
        {
            var result = _context.Songs
                .Include("metadata")
                .Include("uploader")
                .Include("versions")
                .FirstOrDefault(x => x.id == id);

            return result;
        }

        public List<Song> GetAllSongs()
        {
            var result = _context.Songs
                    .Include("metadata")
                    .Include("uploader")
                    .Include("versions")
                    .ToList();

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
                try
                {
                    var dbSong = context.Songs.Where(x => x.id == song.id).Include(y => y.tags).Include(y => y.stats).FirstOrDefault();
                    if (dbSong != null)
                    {
                        dbSong.description = song.description;
                        dbSong.name = song.name;
                        dbSong.stats.plays = song.stats.plays;
                        dbSong.stats.downVotes = song.stats.downVotes;
                        dbSong.stats.downloads = song.stats.downloads;
                        dbSong.stats.upVotes = song.stats.upVotes;
                        dbSong.stats.score = song.stats.score;
                        dbSong.qualified = song.qualified;
                        dbSong.ranked = song.ranked;
                        dbSong.updatedAt = song.updatedAt;
                        dbSong.lastPublishedAt = song.lastPublishedAt;
                        if (dbSong.tags != null && song.tags != null)
                        {
                            var ss = dbSong.tags.ExceptBy(song.tags.Select(y=> y.Name), x => x.Name).Select(z => z.Name).ToList();
                            dbSong.tags.RemoveAll(x =>ss.Contains(x.Name));
                        }
                        if (song.tags != null)
                        {
                            foreach (var s in song.tags)
                            {
                                var ss = dbSong.tags?.FirstOrDefault(x => x.Name == s.Name);
                                if (ss == null)
                                {
                                    dbSong.tags.Add(new Models.DetailedModels.Tag
                                    {
                                        Name = s.Name,
                                        SongId = dbSong.SongId
                                    });
                                }
                            }
                        }
                    }
                    else
                    {
                        // ensure the uploader is correct as uploaders can upload many songs
                        var dbUploader = context.Uploaders.FirstOrDefault(x => x.id == song.uploader.id);
                        song.uploader = dbUploader ?? song.uploader;
                        context.Songs.Add(song);
                    }
                    context.SaveChanges(); // save now so the uploader isn't duplicated!
                }
                catch(Exception e)
                {
                    _logger.LogError("Song {Songnaem} could not be saved. {Message}\n {Stack}", song.name, e.Message, e.StackTrace);
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
