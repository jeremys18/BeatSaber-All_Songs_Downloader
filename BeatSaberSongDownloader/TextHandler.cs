using BeatSaberSongDownloader.Data.Models.BareModels;

namespace BeatSaberSongDownloader
{
    internal static class TextHandler
    {
        internal static string GetValidFileName(string basePath, Song song)
        {
            var filePath = $@"{basePath}\{song.FileName}";

            return filePath;
        }
    }
}
