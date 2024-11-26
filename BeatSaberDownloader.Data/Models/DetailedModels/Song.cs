using BeatSaberDownloader.Data.Models.DetailedModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeatSaberSongDownloader.Data.Models.DetailedModels
{
    [Table("Song")]
    public class Song
    {
        [Key]
        public int SongId { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime uploaded { get; set; }
        public bool automapper { get; set; }
        public bool ranked { get; set; }
        public bool qualified { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public DateTime lastPublishedAt { get; set; }
        public bool bookmarked { get; set; }
        public string declaredAi { get; set; }
        public bool blRanked { get; set; }
        public bool blQualified { get; set; }
        

        public int uploaderId { get; set; }
        public int metadataId { get; set; }
        public int statsId { get; set; }

        // Virtuals
        public virtual Uploader uploader { get; set; }
        public virtual Metadata metadata { get; set; }
        public virtual Stats stats { get; set; }
        public virtual List<Version> versions { get; set; }
        public virtual List<Tag>? tags { get; set; }


        public override string ToString()
        {
            var item = $"{metadata.songName} - {metadata.songAuthorName}";
            return item;
        }
    }
}
