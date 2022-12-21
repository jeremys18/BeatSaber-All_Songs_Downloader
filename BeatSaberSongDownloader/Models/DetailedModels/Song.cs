using System.Collections.Generic;

namespace BeatSaberSongDownloader.Models.DetailedModels
{
    public class Song
    {
        public int SongId { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string uploaded { get; set; }
        public bool automapper { get; set; }
        public bool ranked { get; set; }
        public bool qualified { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
        public string lastPublishedAt { get; set; }

        public int uploaderId { get; set; }
        public int metadataId { get; set; }
        public int statsId { get; set; }

        // Virtuals
        public virtual Uploader uploader { get; set; }
        public virtual Metadata metadata { get; set; }
        public virtual Stats stats { get; set; }
        public virtual List<Version> versions { get; set; }

        public override string ToString()
        {
            var item = $"{metadata.songName} - {metadata.songAuthorName}";
            return item;
        }
    }
}
