﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using BeatSaberDownloader.Data;
using BeatSaberSongDownloader.Data.Models.BareModels;
using Newtonsoft.Json;

namespace BeatSaberSongDownloader
{
    public class Downloader
    {
        public async Task DownloadAllForRangeAsync(string songFolderBasePath, MainWindow mainWindow, List<Song> songs, bool continueRetrying)
        {
            var retrySongs = new List<Song>();
            var songsToGetFromOurServer = new List<Song>();
            var numberOfExistingSongs = 0;
            foreach (var song in songs)
            {
                try
                {
                    if (File.Exists(song.FileName))
                    {
                        numberOfExistingSongs++;
                    }
                    else
                    {
                        using (var wc = new WebClient())
                        {
                            wc.Headers.Add(Consts.UserAgentHeaderName, Consts.UserAgentHeaderValue);
                            wc.Headers.Add(Consts.AcceptLangHeaderName, Consts.AcceptLangHeaderValue);
                            wc.Headers.Add(Consts.SecFetchHeaderName, Consts.SecFetchHeaderValue);
                            wc.DownloadFile(song.BeatSaverDownloadUrl, song.FileName);
                        }

                        if (!File.Exists(song.FileName))
                        {
                            // something went wrong here. Try from our server
                            mainWindow.UpdateTextBox($"\nCould not download file {song.Name} directly from BeatSaver servers. Lets try from our own server....");
                            songsToGetFromOurServer.Add(song);
                        }
                    }
                }
                catch (WebException e)
                {
                    HttpStatusCode code = HttpStatusCode.Ambiguous;
                    var resp = e.Response == null ? null : new StreamReader(e.Response.GetResponseStream()).ReadToEnd().ToLower();
                    if(resp != null)
                    {
                      code  = ((HttpWebResponse)e.Response).StatusCode;
                    }
                     
                    if (code == HttpStatusCode.NotFound)
                    {
                        mainWindow.UpdateTextBox($"\n\nServer responded with 404 not found for song {song.Name}. Will try to get our server....\n");
                        songsToGetFromOurServer.Add(song);
                    }
                    else if (resp != null && resp.Contains("rate limit"))
                    {
                        mainWindow.UpdateTextBox($"\nServer says you're downlaoding too fast. Will wait 20 seconds then continue....\n");
                        Thread.Sleep(20000);
                        retrySongs.Add(song);
                    }
                    else if (code != HttpStatusCode.Forbidden && e.Status != WebExceptionStatus.ConnectFailure)
                    {
                        retrySongs.Add(song);
                        mainWindow.UpdateTextBox($"\nError downloading {song.Name}. Most likely timed out. Song qued for retry....\n");
                    }
                    else
                    {
                        mainWindow.UpdateTextBox($"\nUnknown error downloading {song.Name}. Can't download. Will try getting from our server instead....\n");
                        songsToGetFromOurServer.Add(song);
                    }
                }
            }
            if (retrySongs.Count != 0 && continueRetrying)
            {
                mainWindow.UpdateTextBox($"\n{retrySongs.Count} songs failed to download. Retrying to get them from BeatSaver now....");
                await DownloadAllForRangeAsync(songFolderBasePath, mainWindow, retrySongs, false);
            }
            else if(retrySongs.Count != 0)
            {
                mainWindow.UpdateTextBox($"\nRetried to get {retrySongs.Count} songs but they failed again. To keep things moving will try our server instead....");
                await DownloadAllForRangeFromOurServerAsync(songFolderBasePath, mainWindow, retrySongs);
            }

            if(songsToGetFromOurServer.Count != 0)
            {
                await DownloadAllForRangeFromOurServerAsync(songFolderBasePath, mainWindow, songsToGetFromOurServer);
            }

            if (numberOfExistingSongs > 0)
            {
                mainWindow.UpdateTextBox($"\nSkipped {numberOfExistingSongs} songs as they are already downloaded.");
            }
            mainWindow.UpdateTextBox($"\nCompleted a thread. This thread had {songs.Count} songs....");
        }

        public async Task DownloadAllForRangeFromOurServerAsync(string songFolderBasePath, MainWindow mainWindow, List<Song> songs)
        {
            var songsWithFatelErrors = new List<Song>();
            // We know our server isnt as picky and shouldnt have any limits. So this will be much easier....
            foreach (var song in songs) {
                try
                {
                    var ourServerDownloadUrl = $@"{Consts.OurServerBaseDownloadUrl}/song/{song.Id}/{song.VersionHash}";

                    using (var wc = new WebClient())
                    {
                        // Add auth guids here to lower rouge requests. yes, I know you can see this but this isnt a super secure app....yet. If people spam my server i'll lock it down
                        wc.Headers.Add(Consts.AppTokenHeaderName, Consts.AppTokenValue);
                        wc.Headers.Add(Consts.YoloHoloHeaderName, Consts.YoloHoloHeaderValue);
                        wc.DownloadFile(ourServerDownloadUrl, song.FileName);
                    }

                    if (!File.Exists(song.FileName))
                    {
                        // something went wrong here. Skip song
                        mainWindow.UpdateTextBox($"\nCould not download file {song.Name} directly from BeatSaver servers or our servers. This shouldnt happen....");
                        songsWithFatelErrors.Add(song);
                        mainWindow.AddSongToErrorList(song);
                    }
                }
                catch (WebException e)
                {
                    mainWindow.UpdateTextBox($"\nCould not download file {song.Name} directly from BeatSaver servers or our servers. This shouldnt happen....");
                    songsWithFatelErrors.Add(song);
                    mainWindow.AddSongToErrorList(song);
                }
            }
        }

        internal async Task<List<Song>?> GetAllSongInfo(MainWindow mainWindow, string basePath)
        {
            var result = new List<Song>();

            try
            {
                // Make call to our server to get all the song info
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync($"{Consts.OurServerBaseDownloadUrl}/song/allsongs?basePath={HttpUtility.UrlEncode(basePath)}");

                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<List<Song>>(content);
                }
            }
            catch (Exception e)
            {
                mainWindow.UpdateTextBox($"\n\nTried to call our server but ran into error: {e.Message}.....");
            }


            return result;
        }
    }
}
