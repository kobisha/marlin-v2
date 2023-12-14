using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Marlin.sqlite.Models
{
    public class RetroBonusPlanRanges
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string RetroBonusID { get; set; }

        
        public string? RangeNo { get; set; }

        
        public string? RangeName { get; set; }

        [System.Text.Json.Serialization.JsonConverter(typeof(NullableDecimalConverter))]
        public decimal? RangePercent { get; set; }
    }
}
