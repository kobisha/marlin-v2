namespace Marlin.sqlite.Models
{
    public class SLAByVendors
    {
        public int id { get; set; } 
        public string Vendor { get; set; }
        public string OrderDate { get; set; }
        public decimal Orders { get; set; }
        public decimal Amount { get; set; }
        
        public decimal SLAByQuantity { get; set; }
        public decimal SLAByAmount { get; set; }
        public decimal InTimeOrders { get; set; }
    }
}
