namespace Marlin.sqlite.Models
{
    public class RBFront
    {
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
        public decimal? PlanAmount { get; set; }
        public decimal? RetroPercent { get; set; }
    }
}
