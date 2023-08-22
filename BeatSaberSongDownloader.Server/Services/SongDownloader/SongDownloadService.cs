using BeatSaberDownloader.Data;
using BeatSaberDownloader.Data.Repositories;
using BeatSaberSongDownloader.Server.Services.Base;
using System.Diagnostics;

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
            var sw = new Stopwatch();
            
            _logger.LogInformation($"Song Download Service is running! Started at {DateTime.Now.ToShortTimeString()}");
            try
            {
                sw.Start();
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
                _logger.LogInformation($"An error has occured while grabbing the list of song : {e.Message} \n  {e.InnerException?.Message ?? string.Empty}");
                return Task.FromException(e);
            }
            finally 
            { 
                sw.Stop(); 
            }
            _logger.LogInformation($"Song Downloader completed this round at {DateTime.Now.ToShortTimeString()}. It took \n{sw.Elapsed.Hours} hours\n{sw.Elapsed.Minutes} minutes\n{sw.Elapsed.Seconds} seconds\nTill tomrrow......");
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Song Download Service is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}
