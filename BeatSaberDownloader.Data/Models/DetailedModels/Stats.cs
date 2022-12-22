using System.ComponentModel.DataAnnotations.Schema;

namespace BeatSaberSongDownloader.Data.Models.DetailedModels
{
    [Table("Stats")]
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
