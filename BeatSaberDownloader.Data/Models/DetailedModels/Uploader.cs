using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeatSaberSongDownloader.Data.Models.DetailedModels
{
    [Table("Uploader")]
    public class Uploader
    {
        [Key]
        public int UploaderId { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string avatar { get; set; }
        public string type { get; set; }
        public bool admin { get; set; }
        public bool curator { get; set; }
        // new
        public bool seniorCurator { get; set; }
        public bool playlistUrl { get; set; }
    }
}
