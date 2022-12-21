using System.ComponentModel.DataAnnotations.Schema;

namespace Beat_Saber_All_Songs_Downloader.Models.Legacy
{
    [Table("Difficulties")]
    public class Difficulties
    {
        public int Id { get; set; }
        public bool easy { get; set; }
        public bool normal { get; set; }
        public bool hard { get; set; }
        public bool expert { get; set; }
        public bool expertPlus { get; set; }
    }
}
