namespace Marlin.sqlite.Models
{
    public class SLAByOrder
    {
        public int id { get; set; }
        public string OrderNumber { get; set; }
        public string OrderDate { get; set; }
        public string Shop { get; set; }
        public decimal SLAByQuantity { get; set; }
        public decimal SLAByAmount { get; set; }
        public decimal InTimeOrders { get; set; }
    }
}
