namespace BeatSaber___All_Songs_Downloader.Models
{
    public class BeatSaverServerResponseModel
    {
        public int Code { get; set; }
        public string Identifier { get; set; }
        public string Bucket { get; set; }
        public long Reset { get; set; }
        public int ResetAfter { get; set; }
    }
}
