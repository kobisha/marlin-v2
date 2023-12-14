namespace Marlin.sqlite.Models
{
    public class OrderDetailsUpdate
    {
        public string OrderID { get; set; }
        public string Barcode { get; set; }
        public decimal? ReservedQuantity { get; set; }
    }
}
