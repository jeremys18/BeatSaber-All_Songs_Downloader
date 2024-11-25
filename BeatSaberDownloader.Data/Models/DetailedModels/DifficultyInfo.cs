using System.ComponentModel.DataAnnotations.Schema;

namespace BeatSaberSongDownloader.Data.Models.DetailedModels
{
    [Table("DifficultyInfo")]
    public class DifficultyInfo
    {
        public int Id { get; set; }
        public int versionId { get; set; }
        public int paritySummaryId { get; set; }
        public double njs { get; set; }
        public double offset { get; set; }
        public int notes { get; set; }
        public int bombs { get; set; }
        public int obstacles { get; set; }
        public double nps { get; set; }
        public double length { get; set; }
        public string characteristic { get; set; }
        public string difficulty { get; set; }
        public int events { get; set; }
        public bool chroma { get; set; }
        public bool me { get; set; }
        public bool ne { get; set; }
        public bool cinema { get; set; }
        public double seconds { get; set; }
        public int maxScore { get; set; }

        // new
        public string label { get; set; }

        public virtual ParitySummary paritySummary { get; set; }
        public virtual Version Version { get; set; }
    }
}
