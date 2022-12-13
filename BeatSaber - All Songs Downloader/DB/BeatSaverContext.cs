using Beat_Saber_All_Songs_Downloader.Models;
using System.Data.Entity;

namespace BeatSaber_All_Songs_Downloader.DB
{
    class BeatSaverContext : DbContext
    {
        public DbSet<Verison> Verisons { get; set; }
        public DbSet<ParitySummary> ParitySummaries { get; set; }
        public DbSet<DifficultyInfo> DifficultyInfos { get; set; }
        public DbSet<Metadata> Metadatas { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Stats> Stats { get; set; }
        public DbSet<Uploader> Uploaders { get; set; }
    }
}
