using System.Collections.Generic;

namespace BeatSaber_All_Songs_Downloader
{
    internal static class Consts
    {
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
    }
}
