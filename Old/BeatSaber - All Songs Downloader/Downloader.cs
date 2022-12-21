using Beat_Saber_All_Songs_Downloader.Models;
using BeatSaber___All_Songs_Downloader;
using BeatSaber___All_Songs_Downloader.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BeatSaber_All_Songs_Downloader
{
    public class Downloader
    {
        private MainWindow _mainWindow;

        public async Task DownloadAllForRangeAsync(string songFolderBasePath, MainWindow mainWindow, List<Song> docs)
        {
            _mainWindow = mainWindow;
            var retrySongs = new List<Song>();
            var numberOfExistingSongs = 0;
            foreach (var song in docs) 
            {
                foreach (var ver in song.versions)
                {
                    try
                    {
                        var fileName = TextHandler.GetValidFileName(songFolderBasePath, song);

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
                                mainWindow.UpdateTextBox($"\nCould not download file {fileName}. Will try again....");
                            }
                        }
                    }
                    catch (WebException e)
                    {
                        var resp = e.Response == null ? null : new StreamReader(e.Response.GetResponseStream()).ReadToEnd().ToLower();
                        var code = ((HttpWebResponse)e.Response).StatusCode;
                        if (code == HttpStatusCode.NotFound)
                        {
                            mainWindow.UpdateTextBox($"\n\nServer responded with 404 not found for song {song.name}. Can't download. Skipping song....\n");
                            mainWindow.AddSongToErrorList(song);
                        }
                        else if (resp != null && resp.Contains("rate limit"))
                        {
                            mainWindow.UpdateTextBox($"\n\nServer says you're downlaoding too fast. Will wait 20 seconds then continue....\n");
                            Thread.Sleep(20000);
                            retrySongs.Add(song);
                        }
                        else if (code != HttpStatusCode.Forbidden && e.Status != WebExceptionStatus.ConnectFailure)
                        {
                            retrySongs.Add(song);
                            mainWindow.UpdateTextBox($"\n\nError downloading {song.name}. Most likely timed out. Song qued for retry....\n");
                        }
                        else
                        {
                            mainWindow.UpdateTextBox($"\n\nUnknown error downloading {song.name}. Can't download. Skipping song....\n");
                            mainWindow.AddSongToErrorList(song);
                        }
                    }
                }
            }
            if(retrySongs.Count != 0)
            {
                mainWindow.UpdateTextBox($"\n{retrySongs.Count} songs failed to download. Retrying to get them now....");
                await DownloadAllForRangeAsync(songFolderBasePath, mainWindow, retrySongs);
            }
            else if(numberOfExistingSongs > 0)
            {
                mainWindow.UpdateTextBox($"\nSkipped {numberOfExistingSongs} songs as they are already downloaded.");
            }
            mainWindow.UpdateTextBox("\nCompleted a thread");
        }

        internal async Task<PageResult> GetAllSongInfoForAllFiltersAsync(MainWindow mainWindow)
        {
            PageResult responce = null;
            foreach(var filter in Consts.FilterOptions)
            {
                mainWindow.UpdateTextBox($"\nGetting songs by {filter}....\n\n");
                var songData = await GetAllSongInfoAsync(mainWindow, filter);
                if(responce == null)
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


        internal async Task<PageResult> GetAllSongInfoAsync(MainWindow mainWindow, string filterValue)
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
                    if(info2.docs  == null || info2.docs.Count == 0)
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
                    if(serverResponse.Code == 5)
                    {
                        int seconds = (serverResponse.ResetAfter/1000) + 1; // Add one to account for less than 1 and rounding
                        mainWindow.UpdateTextBox($"\nRate limit Exceeded. Will continue in {seconds} seconds. Got {i + 1} pages so far....");
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
