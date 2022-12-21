using System.ComponentModel.DataAnnotations.Schema;

namespace Beat_Saber_All_Songs_Downloader.Models.Legacy
{
    [Table("Uploader")]
    public class Uploader
    {
        public int Id { get; set; }
        public string _id { get; set; }
        public string username { get; set; }
    }
}
