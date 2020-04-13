
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beat_Saber_All_Songs_Downloader.Models
{
    [Table("Song")]
    public class Song
    {
        public int Id { get; set; }
        public string _id { get; set; }
        public string key { get; set; }
        public string hash { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string directDownload { get; set; }
        public string downloadURL { get; set; }
        public string coverURL { get; set; }
        public string uploaded { get; set; }
        public string deletedAt { get; set; }
        public int uploaderId { get; set; }
        public int metadataId { get; set; }
        public int statsId { get; set; }

        public virtual Uploader uploader { get; set; }
        public virtual Metadata metadata { get; set; }
        public virtual Stats stats { get; set; }


        public override string ToString()
        {
            var item = $"{metadata.songName} - {metadata.songAuthorName}";
            return item;
        }
    }
}
