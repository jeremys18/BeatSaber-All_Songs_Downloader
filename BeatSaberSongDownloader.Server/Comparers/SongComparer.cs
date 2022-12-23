using BeatSaberSongDownloader.Data.Models.DetailedModels;

namespace BeatSaberSongDownloader.Server.Comparers
{
    class SongComparer : IEqualityComparer<Song>
    {
        public bool Equals(Song x, Song y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return x.id.Equals(y.id, StringComparison.InvariantCulture);
        }

        public int GetHashCode(Song obj)
        {
            if (obj == null) return 0;
            return obj.id.GetHashCode();
        }
    }
}
