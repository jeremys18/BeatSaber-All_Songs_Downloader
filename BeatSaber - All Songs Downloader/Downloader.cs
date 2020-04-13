using Beat_Saber_All_Songs_Downloader.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BeatSaber_All_Songs_Downloader
{
    public class Downloader
    {
        private MainWindow _mainWindow;
        private Consts _consts;

        public Downloader()
        {
            _consts = new Consts();
        }

        public async Task DownloadAllForRangeAsync(string songFolderBasePath, MainWindow mainWindow, List<Song> docs)
        {
            _mainWindow = mainWindow;
            var retrySongs = new List<Song>();
            var numberOfExistingSongs = 0;
            foreach (var song in docs) 
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
                            wc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.163 Safari/537.36");
                            wc.DownloadFile($"{_consts.beatSaverBaseUrl}/{song.directDownload}", fileName);
                        }

                        if (!File.Exists(fileName))
                        {
                            mainWindow.UpdateTextBox($"\nCould not download file {fileName}. Will try again....");
                        }
                    }
                }
                catch (WebException e)
                {
                    var resp = new StreamReader(e.Response.GetResponseStream()).ReadToEnd().ToLower();
                    var code = ((HttpWebResponse)e.Response).StatusCode;
                    if (code == HttpStatusCode.NotFound)
                    {
                        mainWindow.UpdateTextBox($"\n\nServer responded with 404 not found for song {song.name}. Can't download. Skipping song....\n");
                        mainWindow.AddSongToErrorList(song);
                    }
                    else if (resp.Contains("rate limit"))
                    {
                        mainWindow.UpdateTextBox($"\n\nServer says you're downlaoding too fast. Will wait 20 seconds then continue....\n");
                        Thread.Sleep(20000);
                        retrySongs.Add(song);
                    }
                    else if(code != HttpStatusCode.Forbidden && e.Status != WebExceptionStatus.ConnectFailure)
                    {
                        retrySongs.Add(song);
                    }
                    else
                    {
                        mainWindow.UpdateTextBox($"\n\nUnknown error downloading {song.name}. Can't download. Skipping song....\n");
                        mainWindow.AddSongToErrorList(song);
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

        internal async Task<PageResult> GetAllSongInfoAsync(MainWindow mainWindow)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.163 Safari/537.36");
            var response = await client.GetAsync($"{_consts.pageBaseUrl}/1");
            var json = await response.Content.ReadAsStringAsync();
            var info = JsonConvert.DeserializeObject<PageResult>(json);
            var pageCount = (info.totalDocs % info.docs.Count) == 0 ? (info.totalDocs / info.docs.Count) : (info.totalDocs / info.docs.Count) + 1;

            for (int i = 1; i <= pageCount; i++)
            {
                try
                {
                    response = await client.GetAsync($"{_consts.pageBaseUrl}/{i}");
                    json = await response.Content.ReadAsStringAsync();
                    var info2 = JsonConvert.DeserializeObject<PageResult>(json);
                    info.docs.AddRange(info2.docs);
                }
                catch (Exception e)
                {
                    if(json.ToLower().Contains("rate limit"))
                    {
                        int seconds = 20;
                        mainWindow.UpdateTextBox($"\nRate limit Exceeded. Will continue in 20 seconds. Got {i} out of {pageCount}....");
                        i--;
                        Thread.Sleep(seconds * 1000);
                    }
                    else
                    {
                        var j = "";
                    }
                }
            }


            return info;
        }


        internal async Task<int> GetTotalCountOfSongsAsync()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.163 Safari/537.36");
            var response = await client.GetAsync($"{_consts.pageBaseUrl}/1");
            var json = await response.Content.ReadAsStringAsync();
            var info = JsonConvert.DeserializeObject<PageResult>(json);
            return info.totalDocs;
        }
    }
}
