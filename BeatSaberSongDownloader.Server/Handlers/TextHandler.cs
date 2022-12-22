using BeatSaberSongDownloader.Data.Models.DetailedModels;

namespace BeatSaberSongDownloader.Server.Handlers
{
    internal static class TextHandler
    {
        internal static string GetValidFileName(string basePath, Song song)
        {
            var fileName = $"{song.id} - ({song.name} - {song.metadata.songAuthorName} [{song.uploader.name}]).zip";
            var filePath = $@"{basePath}\{ReplaceInvalidChars(fileName)}";

            if (filePath.Length > 260)
            {
                fileName = $"{song.id} - {song.name}.zip";
                filePath = $@"{basePath}\{ReplaceInvalidChars(fileName)}";

                if ($@"{basePath}\{fileName}".Length > 260)
                {
                    fileName = $"{song.id} - Song name too long.zip";
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
