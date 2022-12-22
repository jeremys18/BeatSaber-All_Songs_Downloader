using BeatSaberSongDownloader.Data.Models.DetailedModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Version = BeatSaberSongDownloader.Data.Models.DetailedModels.Version;

namespace BeatSaberDownloader.Data.DBContext
{
    public class BeatSaverContext : DbContext
    {
        public IConfiguration _configuration { get; set; }

        public BeatSaverContext(IConfiguration configuration) : base()
        {
            _configuration = configuration;
        }

        public DbSet<Version>? Versions { get; set; }
        public DbSet<ParitySummary>? ParitySummaries { get; set; }
        public DbSet<DifficultyInfo>? DifficultyInfos { get; set; }
        public DbSet<Metadata>? Metadatas { get; set; }
        public DbSet<Song>? Songs { get; set; }
        public DbSet<Stats>? Stats { get; set; }
        public DbSet<Uploader>? Uploaders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("BeatSaverContext"), sqlServerOptions => sqlServerOptions.CommandTimeout(3600));
        }
    }
}
