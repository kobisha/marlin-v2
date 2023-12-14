namespace Marlin.sqlite.Models
{
    public class SLAByCategory
    {
        public int id { get; set; }
        public string ProductCategory { get; set; }
        public decimal Orders { get; set; }
        public decimal OrderedQuantity { get; set; }
        public decimal OrderedAmount { get; set; }
        public decimal SLAByQuantity { get; set; }
        public decimal SLAByAmount { get; set; }
        public decimal InTimeOrders { get; set; }
    }
}
