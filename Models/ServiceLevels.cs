namespace Marlin.sqlite.Models
{
    public class ServiceLevels
    {
        public int id { get; set; }
        public string? Vendor { get; set; }
        public string? Shop { get; set; }
        public string? ProductCategory { get; set; }
        public string? OrderNumber { get; set; }
        public string? OrderDate { get; set; }
        public string? Product { get; set; }
        public decimal?  OrderedQuantity { get; set; }
        public decimal? OrderedAmount { get; set; }
        public decimal? DeliveredQuantity { get; set; }
        public decimal? DeliveredAmount { get; set; }
        public decimal? SLAByQuantity { get; set; }
        public decimal? SLAByAmount { get; set; }
        public int InTimeOrders { get; set; }
    }
}
