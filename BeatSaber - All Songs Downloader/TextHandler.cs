using Beat_Saber_All_Songs_Downloader.Models;
using System.IO;

namespace BeatSaber_All_Songs_Downloader
{
    internal static class TextHandler
    {
        internal static string GetValidFileName(string basePath, Song song)
        {
            var fileName = $"{song.key} - ({song.name} - {song.metadata.songAuthorName} [{song.uploader.username}]).zip";
            var filePath = $@"{basePath}\{ReplaceInvalidChars(fileName)}";

            if (filePath.Length > 260)
            {
                fileName = $"{song.key} - {song.name}.zip";
                filePath = $@"{basePath}\{ReplaceInvalidChars(fileName)}";

                if ($@"{basePath}\{fileName}".Length > 260)
                {
                    fileName = $"{song.key} - Song name too long.zip";
                    filePath = $@"{basePath}\{ReplaceInvalidChars(fileName)}";
                }
            }

            return filePath;
        }

        internal static string ReplaceInvalidChars(string filename)
        {
            return string.Join(" ", filename.Split(Path.GetInvalidFileNameChars()));
        }
    }
}
