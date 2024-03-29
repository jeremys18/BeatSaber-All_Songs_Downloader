﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beat_Saber_All_Songs_Downloader.Models
{
    [Table("Version")]
    public class Version
    {
        public int Id { get; set; }
        public int SongId { get; set; }
        public string hash { get; set; }
        public string state { get; set; }
        public DateTime createdAt { get; set; }
        public decimal sageScore { get; set; }
        public string downloadURL { get; set; }
        public string coverURL { get; set; }
        public string previewURL { get; set; }
         
        public virtual List<DifficultyInfo> diffs { get; set; }
        public Song Song { get; set; }
    }
}
