using System.ComponentModel.DataAnnotations.Schema;

namespace BeatSaberSongDownloader.Data.Models.DetailedModels
{
    [Table("ParitySummary")]
    public class ParitySummary
    {
        public int Id { get; set; }
        public int errors { get; set; }
        public int warns { get; set; }
        public int resets { get; set; }
    }
}
