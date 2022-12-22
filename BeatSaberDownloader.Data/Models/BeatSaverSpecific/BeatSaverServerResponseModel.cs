using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatSaberDownloader.Data.Models.BeatSaverSpecific
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
