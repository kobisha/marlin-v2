namespace Marlin.sqlite.Models
{
    public class InvoiceData
    {
        public string OrderID { get; set; }
        public string InvoiceID { get; set; }
        public DateTime Date { get; set; }
        public string Number { get; set; }
        public string Amount { get; set; }
        public string WaybillNumber { get; set; }
        public List<ProductData> Products { get; set; }
    }
}
