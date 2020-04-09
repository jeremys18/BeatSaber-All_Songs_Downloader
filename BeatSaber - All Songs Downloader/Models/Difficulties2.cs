using System.ComponentModel.DataAnnotations.Schema;

namespace Beat_Saber_All_Songs_Downloader.Models
{
    [Table("Difficulties2")]
    public class Difficulties2
    {
        public int Id { get; set; }
        public int? easyId { get; set; }
        public int? normalId { get; set; }
        public int? hardId { get; set; }
        public int? expertId { get; set; }
        public int? expertPlusId { get; set; }


        public virtual DifficultyInfo easy { get; set; }
        public virtual DifficultyInfo normal { get; set; }
        public virtual DifficultyInfo hard { get; set; }
        public virtual DifficultyInfo expert { get; set; }
        public virtual DifficultyInfo expertPlus { get; set; }
    }
}
