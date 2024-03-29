﻿using System.ComponentModel.DataAnnotations.Schema;

namespace BeatSaberSongDownloader.Data.Models.DetailedModels
{
    [Table("Metadata")]
    public class Metadata
    {
        public int Id { get; set; }
        public decimal bpm { get; set; }
        public int duration { get; set; }
        public string songName { get; set; }
        public string songSubName { get; set; }
        public string songAuthorName { get; set; }
        public string levelAuthorName { get; set; }
    }
}
