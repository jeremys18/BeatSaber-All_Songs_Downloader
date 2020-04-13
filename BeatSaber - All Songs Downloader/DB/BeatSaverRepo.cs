using Beat_Saber_All_Songs_Downloader.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BeatSaber_All_Songs_Downloader.DB
{
    public class BeatSaverRepo : System.IDisposable
    {
        private bool isDisposed;
        private BeatSaverContext _context;

        public BeatSaverRepo()
        {
            _context = new BeatSaverContext();
        }

        public List<Song> GetAllSongs()
        {
            var result = new List<Song>();

            using(var context = _context)
            {
                result = _context.Songs
                    .Include("Metadata")
                    .Include("Uploader")
                    .ToList();
            }

            return result;
        }

        public void SaveSongsToDb(List<Song> songs)
        {
            using (var context = _context)
            {
                context.Songs.AddRange(songs);
                context.SaveChanges();
            }
        }

        public void UpdateSongStats(List<Song> songs)
        {
            using (var context = _context)
            {
                foreach(var song in songs)
                {
                    var dbSongStats = _context.Songs.First(x => x._id == song._id).stats;
                    dbSongStats.downloads = song.stats.downloads;
                    dbSongStats.downVotes = song.stats.downVotes;
                    dbSongStats.heat = song.stats.heat;
                    dbSongStats.plays = song.stats.plays;
                    dbSongStats.rating = song.stats.rating;
                    dbSongStats.upVotes = song.stats.upVotes;
                }
                context.SaveChanges();
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
