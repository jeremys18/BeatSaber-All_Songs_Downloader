using System.ComponentModel.DataAnnotations.Schema;

namespace Beat_Saber_All_Songs_Downloader.Models.Legacy
{
    [Table("Characteristic")]
    public class Characteristic
    {
        public int Id { get; set; }
        public string name { get; set; }
        public int Difficulties2Id { get; set; }
        public int MetadataId { get; set; }

        public virtual Difficulties2 difficulties { get; set; }
        public virtual Metadata Metadata { get; set; }
    }
}
