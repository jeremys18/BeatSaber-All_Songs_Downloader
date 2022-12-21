using Beat_Saber_All_Songs_Downloader.Models;
using BeatSaber_All_Songs_Downloader;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace BeatSaber___All_Songs_Downloader.Helpers
{
    public static class FileHelper
    {
        public static  List<Song> FindMissingSongFiles(string basePath, List<Song> songs, MainWindow window)
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

        public static void SavePageResultToFile(PageResult pageResult)
        {
            string roaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string saveFolderLocaion = $@"{roaming}\BeatSaber - All Songs Downloader";
            if (!Directory.Exists(saveFolderLocaion))
            {
                Directory.CreateDirectory(saveFolderLocaion);
            }
            var json = JsonConvert.SerializeObject(pageResult);
            File.WriteAllText($@"{saveFolderLocaion}\songs.json", json);
        }

        public static PageResult GetPageResultFromFile()
        {
            string roaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string saveFolderLocaion = $@"{roaming}\BeatSaber - All Songs Downloader";
            if (!Directory.Exists(saveFolderLocaion))
            {
                return new PageResult { docs = new List<Song>()};
            }
            var json = File.ReadAllText($@"{saveFolderLocaion}\songs.json");
            var result = JsonConvert.DeserializeObject<PageResult>(json);
            return result;
        }
    }
}
