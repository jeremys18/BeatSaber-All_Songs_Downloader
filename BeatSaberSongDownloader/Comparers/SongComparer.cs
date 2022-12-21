using BeatSaberSongDownloader.Data.Models.BareModels;
using System;
using System.Collections.Generic;

namespace BeatSaberSongDownloader.Comparers
{
    class SongComparer : IEqualityComparer<Song>
    {
        public bool Equals(Song x, Song y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return x.Id.Equals(y.Id, StringComparison.InvariantCulture);
        }

        public int GetHashCode(Song obj)
        {
            if (obj == null) return 0;
            return obj.Id.GetHashCode();
        }
    }
}
