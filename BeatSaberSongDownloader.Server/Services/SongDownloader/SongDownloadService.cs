using BeatSaberDownloader.Data;
using BeatSaberDownloader.Data.Repositories;
using BeatSaberSongDownloader.Server.Services.Base;

namespace BeatSaberSongDownloader.Server.Services.SongDownloader
{
    public class SongDownloadService : CronJobService
    {
        private readonly ILogger<SongDownloadService> _logger;
        private IConfiguration _configuration;

        public SongDownloadService(IScheduleConfig<SongDownloadService> config, ILogger<SongDownloadService> logger, IConfiguration configuration) : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Song Download Service created and awaiting the time to come for it to work work....");
            return base.StartAsync(cancellationToken);
        }

        public override async Task<Task> DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Song Download Service is running!");
            try
            {
                // Do everything we did before in the client but now do it in the background :D>-<
                var downloader = new Downloader(_logger);

                // Get current list of songs from their server
                var latestSongs = await downloader.GetAllSongInfoForAllFiltersAsync();

                //upsert entries
                new BeatSaverRepository(_configuration).SaveSongsToDb(latestSongs.docs);

                // Download all the songs
                await downloader.DownloadAllForRangeAsync(Consts.SaveFolderPath, latestSongs.docs, true);
            }
            catch (Exception e)
            {
                return Task.FromException(e);
            }
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Song Download Service is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}
