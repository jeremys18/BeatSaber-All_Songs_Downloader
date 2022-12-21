namespace BeatSaberSongDownloader.Data.Models.DetailedModels
{
    public class Uploader
    {
        public int UploaderId { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string avatar { get; set; }
        public string type { get; set; }
        public bool admin { get; set; }
        public bool curator { get; set; }
    }
}
