using System.Collections.Generic;

namespace Beat_Saber_All_Songs_Downloader.Models
{
    public class PageResult
    {
        public List<Song> docs { get; set; }
        public int totalDocs { get; set; }
        public int lastPage { get; set; }
        public int prevPage { get; set; }
        public int nextPage { get; set; }
    }
}
