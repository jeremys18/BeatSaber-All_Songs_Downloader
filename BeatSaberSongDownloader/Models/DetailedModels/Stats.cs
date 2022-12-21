namespace BeatSaberSongDownloader.Models.DetailedModels
{
    public class Stats
    {
        public int Id { get; set; }
        public int downloads { get; set; }
        public int plays { get; set; }
        public int downVotes { get; set; }
        public int upVotes { get; set; }
        public double score { get; set; }
    }
}
