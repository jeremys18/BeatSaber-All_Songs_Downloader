namespace BeatSaberSongDownloader.Models.BareModels
{
    // This is a bare bones model. As the client no longer stores anything and we currently only care out the download url and song name, we only need those properties
    // The full pi model from the beatsaver server is under the detailed models folder. It's kept just in case we want to use more properties in the future. Also, our server uses it
    public class Song
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string FileName { get; set; }

        // This URl is for their server, not ours. 
        // If this URL fails we can default to our server url as a backup but first we need to know their URL
        public string BeatSaverDownloadUrl { get; set; }
    }
}
