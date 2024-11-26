
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeatSaberDownloader.Data.Models.DetailedModels
{
    [Table("Tag", Schema ="dbo")]
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        public int SongId { get; set; }
        public string Name { get; set; }
    }
}
