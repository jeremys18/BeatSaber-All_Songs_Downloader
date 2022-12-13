using System.ComponentModel.DataAnnotations.Schema;

namespace Beat_Saber_All_Songs_Downloader.Models.Legacy
{
    [Table("DifficultyInfo")]
    public class DifficultyInfo
    {
        public int Id { get; set; }
        public double duration { get; set; }
        public int length { get; set; }
        public int bombs { get; set; }
        public int notes { get; set; }
        public int obstacles { get; set; }
        public double njs { get; set; }
        public double njsOffset { get; set; }
    }
}
