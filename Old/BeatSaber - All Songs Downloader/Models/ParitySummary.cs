using System.ComponentModel.DataAnnotations.Schema;

namespace Beat_Saber_All_Songs_Downloader.Models
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
