using BeatSaberSongDownloader.Models.BareModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace BeatSaberSongDownloader.Helpers
{
    public static class FileHelper
    {
        public static List<Song> FindMissingSongFiles(string basePath, List<Song> songs, MainWindow window)
        {
            var result = new List<Song>();
            foreach (var song in songs)
            {
                var fileName = TextHandler.GetValidFileName(basePath, song);
                if (!File.Exists(fileName))
                {
                    result.Add(song);
                }
            }

            window.UpdateTextBox($"\n\nFound {result.Count} new songs.");
            return result;
        }

        public static void SaveSongsToFile(List<Song> songs)
        {
            string roaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string saveFolderLocaion = $@"{roaming}\BeatSaber - Songs Downloader";
            if (!Directory.Exists(saveFolderLocaion))
            {
                Directory.CreateDirectory(saveFolderLocaion);
            }
            var json = JsonConvert.SerializeObject(songs);
            File.WriteAllText($@"{saveFolderLocaion}\songs.json", json);
        }

        public static List<Song> GetSongsFromFile()
        {
            string roaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string saveFolderLocaion = $@"{roaming}\BeatSaber - Songs Downloader";
            if (!Directory.Exists(saveFolderLocaion))
            {
                return new List<Song>();
            }
            var json = File.ReadAllText($@"{saveFolderLocaion}\songs.json");
            var result = JsonConvert.DeserializeObject<List<Song>>(json);
            return result ?? new List<Song>();
        }
    }
}
