using BeatSaberSongDownloader.Data.Models.DetailedModels;

namespace BeatSaberDownloader.Data.Models.BeatSaverSpecific
{
    public class PageResult
    {
        public List<Song> docs { get; set; }
        public PageInfo info { get; set; }
    }
}
