using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Marlin.sqlite.Models
{
    public class RetroBonusDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string RetroBonusID { get; set; }

        
        public string Barcode { get; set; }

        [System.Text.Json.Serialization.JsonConverter(typeof(NullableDecimalConverter))]
        public decimal? MinimalPercent { get; set; }

        [System.Text.Json.Serialization.JsonConverter(typeof(NullableDecimalConverter))]
        public decimal? PlanPercent { get; set; }

        [System.Text.Json.Serialization.JsonConverter(typeof(NullableDecimalConverter))]
        public decimal? ManufacturerPercent { get; set; }
       

        public int? RetroBonusHeaderId { get; set; }
        public RetroBonusHeader? RetroBonusHeader { get; set; }
    }
}
