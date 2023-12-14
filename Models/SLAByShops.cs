namespace Marlin.sqlite.Models
{
    public class SLAByShops
    {
        public int id { get; set; }
        public string Shop { get; set; }
        public decimal Orders { get; set; }
        public decimal DeliveredQuantity { get; set; }
        
        public decimal SLAByQuantity { get; set; }
        public decimal SLAByAmount { get; set; }
        public decimal InTimeOrders { get; set; }
    }
}
