using System.Collections.Generic;

namespace Beat_Saber_All_Songs_Downloader.Models.Legacy
{
    public class PageResult
    {
        public List<Song> docs { get; set; }


        //Latest version no longer has these

        //public int totalDocs { get; set; }
        //public int lastPage { get; set; }
        //public int prevPage { get; set; }
        //public int nextPage { get; set; }
    }
}
