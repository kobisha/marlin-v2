using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Marlin.sqlite.Models
{
    public class temTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Barcode { get; set; }
        public string ProductID { get; set; }
        public string Product { get; set; }
        public string Unit { get; set; }
        public string Status { get; set; }
        public decimal? Price { get; set; }
        public decimal? LastPrice { get; set; }
        [System.Text.Json.Serialization.JsonConverter(typeof(NullableDateTimeConverter))]
        public DateTime? LastChangeDate { get; set; }
    }

}
