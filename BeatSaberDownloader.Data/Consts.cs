namespace BeatSaberDownloader.Data
{
    public static class Consts
    {
        public const string OurServerBaseDownloadUrl = "http://141.193.188.213:5000/api"; 
        public const string SaveFolderPath = @"G:\BeatSaberSongs";
        public const string DownloadBasePath = "https://beatsaver.com/api/download/key/";
        public const string StatsBasePath = "https://beatsaver.com/api/stats/key/";
        public const string BeatSaverBaseUrl = "https://beatsaver.com";
        public const string PageBaseUrl = "https://beatsaver.com/api/search/text";
        static public readonly List<string> FilterOptions = new List<string> { "Relevance", "Latest", "Rating", "Curated" };

        public const string UserAgentHeaderName = "User-Agent";
        public const string AcceptLangHeaderName = "Accept-Language";
        public const string SecFetchHeaderName = "sec-fetch-mode";

        public const string UserAgentHeaderValue = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36";
        public const string AcceptLangHeaderValue = "en-US,en-LR;q=0.9,en-IE;q=0.8,en-DE;q=0.7,en-GB;q=0.6,en;q=0.5";
        public const string SecFetchHeaderValue = "navigate";

        // our server headers
        public const string AppTokenHeaderName = "AppyWappy";
        public const string YoloHoloHeaderName = "YoloHolo";

        public const string AppTokenValue = "b19969af-fae1-4c39-aa6a-da00959e20ca";
        public const string YoloHoloHeaderValue = "SiMeHomeyWomey";
    }
}
