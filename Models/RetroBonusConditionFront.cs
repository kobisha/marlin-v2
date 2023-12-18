namespace Marlin.sqlite.Models
{
    public class RetroBonusConditionFront
    {   public int Id { get; set; }
        public string? RangeName { get; set; }
        [System.Text.Json.Serialization.JsonConverter(typeof(NullableDecimalConverter))]
        public decimal? RangePercent { get; set; }
    }
}
