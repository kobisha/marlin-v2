using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using JsonConverter = Newtonsoft.Json.JsonConverter;

namespace Marlin.sqlite.Models
{
    public class RetroBonusHeader
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public string AccountID { get; set; }

        
        public string RetroBonusID { get; set; }

        
        public string DocumentNo { get; set; }

        
        public string SupplierID { get; set; }

        
        public DateTime StartDate { get; set; }
        [System.Text.Json.Serialization.JsonConverter(typeof(NullableDateTimeConverter))]
        public DateTime? EndDate { get; set; }

        
        public string Status { get; set; }

        
        public string Condition { get; set; }

        [System.Text.Json.Serialization.JsonConverter(typeof(StringToBoolConverter))]
        public bool IsMarketing { get; set; }

        [System.Text.Json.Serialization.JsonConverter(typeof(StringToBoolConverter))]
        public bool FundedByManufacturer { get; set; }

        [System.Text.Json.Serialization.JsonConverter(typeof(NullableDecimalConverter))]
        public decimal? MinimalPercent { get; set; }

        [System.Text.Json.Serialization.JsonConverter(typeof(NullableDecimalConverter))]
        public decimal? PlanAmount { get; set; }

        [System.Text.Json.Serialization.JsonConverter(typeof(NullableDecimalConverter))]
        public decimal? PlanPercent { get; set; }

        [System.Text.Json.Serialization.JsonConverter(typeof(NullableDecimalConverter))]
        public decimal? ManufacturerPercent { get; set; }

        
        public List<RetroBonusDetails> Products { get; set; }

       
        public List<RetroBonusPlanRanges> PlanRanges { get; set; }
    }
}
