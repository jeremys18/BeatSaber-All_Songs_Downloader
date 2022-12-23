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
            _logger.LogInformation("Song Download Service starts.");
            return base.StartAsync(cancellationToken);
        }

        public override async Task<Task> DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Yo, I got herrrrrrrrrrrrrrrr!!!!");
            try
            {
                // Do everything we did before in the client but now do it in the background :D>-<
                var downloader = new Downloader();
                _logger.LogInformation("Ooo did I sneak past you???????????????????????????????????");

                // Get current list of songs from their server
                var latestSongs = await downloader.GetAllSongInfoForAllFiltersAsync();

                //upsert entries
                new BeatSaverRepository(_configuration).SaveSongsToDb(latestSongs.docs);

                // Download all the songs
            }
            catch (Exception e)
            {
                return Task.FromException(e);
            }
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} CronJob 3 is working.");
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Song Download Service is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}
