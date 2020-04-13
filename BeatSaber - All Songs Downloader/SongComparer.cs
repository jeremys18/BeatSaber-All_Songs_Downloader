using Beat_Saber_All_Songs_Downloader.Models;
using System;
using System.Collections.Generic;

namespace BeatSaber___All_Songs_Downloader
{
    class SongComparer : IEqualityComparer<Song>
    {
        public bool Equals(Song x, Song y)
        {
            if(x == null || y == null)
            {
                return false;
            }

            return x.key.Equals(y.key, StringComparison.InvariantCulture);
        }

        public int GetHashCode(Song obj)
        {
            if (obj == null) return 0;
            return obj.key.GetHashCode();
        }
    }
}
