using BeatSaberDownloader.Data;
using BeatSaberDownloader.Data.Models.BeatSaverSpecific;
using BeatSaberSongDownloader.Data.Models.DetailedModels;
using BeatSaberSongDownloader.Server.Comparers;
using BeatSaberSongDownloader.Server.Handlers;
using Newtonsoft.Json;
using System.Net;

namespace BeatSaberSongDownloader.Server.Services.SongDownloader
{
    public class Downloader
    {
        ILogger<SongDownloadService> _logger;

        public Downloader(ILogger<SongDownloadService> logger)
        {
            _logger = logger;
        }

        // Because the server grabs songs from the beat saver servers we use the detailed models here to grab everything we can.
        // We can then return to our client only the basic model so the client only gets what we actually use right now
        // TODO: Add logging to the DB so we can track errors...
        public async Task DownloadAllForRangeAsync(string songFolderBasePath, List<Song> docs, bool keepGoing)
        {
            var retrySongs = new List<Song>();
            var numberOfExistingSongs = 0;
            foreach (var song in docs)
            {
                var verNum = 1;
                foreach (var ver in song.versions)
                {
                    try
                    {
                        var fileName = TextHandler.GetValidFileName(songFolderBasePath, song, verNum);

                        if (File.Exists(fileName))
                        {
                            numberOfExistingSongs++;
                        }
                        else
                        {
                            using (WebClient wc = new WebClient())
                            {
                                wc.Headers.Add(Consts.UserAgentHeaderName, Consts.UserAgentHeaderValue);
                                wc.Headers.Add(Consts.AcceptLangHeaderName, Consts.AcceptLangHeaderValue);
                                wc.Headers.Add(Consts.SecFetchHeaderName, Consts.SecFetchHeaderValue);
                                wc.DownloadFile(ver.downloadURL, fileName);
                            }

                            if (!File.Exists(fileName))
                            {
                                _logger.LogInformation($"{song.name} failed to download...");
                            }
                        }
                    }
                    catch (WebException e)
                    {
                        // add logging
                        var resp = e.Response == null ? null : new StreamReader(e.Response.GetResponseStream()).ReadToEnd().ToLower();
                        var code = ((HttpWebResponse)e.Response).StatusCode;
                        if (code == HttpStatusCode.NotFound)
                        {
                            _logger.LogInformation($"\nServer responded with 404 not found for song {song.name}. Can't download. Skipping song....\n");
                        }
                        else if (resp != null && resp.Contains("rate limit"))
                        {
                            _logger.LogInformation($"\nServer says you're downlaoding too fast. Will wait 20 seconds then continue....\n");
                            Thread.Sleep(20000);
                            retrySongs.Add(song);
                        }
                        else if (code != HttpStatusCode.Forbidden && e.Status != WebExceptionStatus.ConnectFailure)
                        {
                            retrySongs.Add(song);
                            _logger.LogInformation($"\nError downloading {song.name}. Most likely timed out. Song qued for retry....\n");
                        }
                        else
                        {
                            _logger.LogInformation($"\nUnknown error downloading {song.name}. Can't download. Skipping song....\n");
                        }
                    }
                    verNum++;
                }
            }
            if (retrySongs.Count != 0 && keepGoing)
            {
                _logger.LogInformation($"{retrySongs.Count} songs failed to download. Retrying to get them now....");
                await DownloadAllForRangeAsync(songFolderBasePath, retrySongs, false);
            }
            else if(retrySongs.Count > 0)
            {
                _logger.LogInformation($"{retrySongs.Count} songs failed to download, even after retrying.....");
            }
        }

        internal async Task<PageResult> GetAllSongInfoForAllFiltersAsync()
        {
            PageResult responce = null;
            foreach (var filter in Consts.FilterOptions)
            {
                _logger.LogInformation($"\nGetting songs by {filter}....");
                var songData = await GetAllSongInfoAsync(filter);
                if (responce == null)
                {
                    responce = songData;
                }
                else
                {
                    var newSongs = songData.docs.Except(responce.docs, new SongComparer()); 
                    responce.docs.AddRange(newSongs);
                }
            }

            return responce;
        }


        internal async Task<PageResult> GetAllSongInfoAsync(string filterValue)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add(Consts.UserAgentHeaderName, Consts.UserAgentHeaderValue);
            client.DefaultRequestHeaders.Add(Consts.AcceptLangHeaderName, Consts.AcceptLangHeaderValue);
            client.DefaultRequestHeaders.Add(Consts.SecFetchHeaderName, Consts.SecFetchHeaderValue);

            var urlToUse = new Uri($"https://beatsaver.com/api/search/text/0?sortOrder={filterValue}", UriKind.Absolute);
            var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, urlToUse)).ConfigureAwait(false);
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var info = JsonConvert.DeserializeObject<PageResult>(json);
            var keepgoing = true;

            for (int i = 1; keepgoing; i++)
            {
                try
                {
                    var urlToUseNow = new Uri($"https://beatsaver.com/api/search/text/{i}?sortOrder={filterValue}", UriKind.Absolute);
                    response = await client.GetAsync(urlToUseNow);
                    json = await response.Content.ReadAsStringAsync();
                    var info2 = JsonConvert.DeserializeObject<PageResult>(json);
                    if (info2.docs == null || info2.docs.Count == 0)
                    {
                        // we've reached the end of the list so stop the loop
                        keepgoing = false;
                    }
                    else
                    {
                        info.docs.AddRange(info2.docs);
                    }

                    Thread.Sleep(334);// Rate limits only allow 90 requests per 30 seconds or it will kick us. That means we have a max of 3 requests a second... so every 334 mili seconds
                }
                catch (Exception e)
                {
                    var serverResponse = JsonConvert.DeserializeObject<BeatSaverServerResponseModel>(json);
                    if (serverResponse.Code == 5)
                    {
                        int seconds = serverResponse.ResetAfter / 1000 + 1; // Add one to account for less than 1 and rounding
                        _logger.LogError($"\nRate limit Exceeded. Will continue in {seconds} seconds. Got {i + 1} pages so far....");
                        i--;
                        Thread.Sleep(seconds * 1000);
                    }
                    else
                    {
                        var j = "";
                    }
                }
            }

            client.Dispose();
            return info;
        }

    }
}
