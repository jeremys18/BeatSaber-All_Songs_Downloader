using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beat_Saber_All_Songs_Downloader.Models.Legacy
{
    [Table("Metadata")]
    public class Metadata
    {
        public int Id { get; set; }
        public string songName { get; set; }
        public string songSubName { get; set; }
        public string songAuthorName { get; set; }
        public string levelAuthorName { get; set; }
        public double bpm { get; set; }
        public int duration { get; set; }
        public int difficultiesId { get; set; }
        public string automapper { get; set; }

        public virtual Difficulties difficulties { get;set;}
        public virtual List<Characteristic> characteristics { get; set; }
    }
}
