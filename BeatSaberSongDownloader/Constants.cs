using System.Collections.Generic;

namespace BeatSaberSongDownloader
{
    internal static class Constants
    {
        internal const string OurServerBaseDownloadUrl = ""; // Need to figure this out
        internal const string DownloadBasePath = "https://beatsaver.com/api/download/key/";
        internal const string StatsBasePath = "https://beatsaver.com/api/stats/key/";
        internal const string BeatSaverBaseUrl = "https://beatsaver.com";
        internal const string PageBaseUrl = "https://beatsaver.com/api/search/text";
        static internal readonly List<string> FilterOptions = new List<string> { "Relevance", "Latest", "Rating", "Curated" };

        internal const string UserAgentHeaderName = "User-Agent";
        internal const string AcceptLangHeaderName = "Accept-Language";
        internal const string SecFetchHeaderName = "sec-fetch-mode";

        internal const string UserAgentHeaderValue = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36";
        internal const string AcceptLangHeaderValue = "en-US,en-LR;q=0.9,en-IE;q=0.8,en-DE;q=0.7,en-GB;q=0.6,en;q=0.5";
        internal const string SecFetchHeaderValue = "navigate";

        // our server headers
        internal const string AppTokenHeaderName = "AppyWappy";
        internal const string YoloHoloHeaderName = "YoloHolo";

        internal const string AppTokenValue = "b19969af-fae1-4c39-aa6a-da00959e20ca";
        internal const string YoloHoloHeaderValue = "SiMeHomeyWomey";

    }
}
