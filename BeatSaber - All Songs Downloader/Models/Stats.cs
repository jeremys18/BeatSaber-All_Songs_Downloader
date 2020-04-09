using System.ComponentModel.DataAnnotations.Schema;

namespace Beat_Saber_All_Songs_Downloader.Models
{
    [Table("Stats")]
    public class Stats
    {
        public int Id { get; set; }
        public int downloads { get; set; }
        public int plays { get; set; }
        public int downVotes { get; set; }
        public int upVotes { get; set; }
        public double heat { get; set; }
        public decimal rating { get; set; }
    }
}
