using Beat_Saber_All_Songs_Downloader.Models;
using BeatSaber_All_Songs_Downloader.DB;
using System.Collections.Generic;

namespace BeatSaber___All_Songs_Downloader.Helpers
{
    public static class BeatSaverDbHelper
    {
        public static void SaveSongsToDb(List<Song> songs)
        {
            using(var repo = new BeatSaverRepo())
            {
                repo.SaveSongsToDb(songs);
            }
        }
    }
}
