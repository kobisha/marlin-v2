using System.ComponentModel.DataAnnotations;

namespace Marlin.sqlite.Models
{
    public class invoicfront
    {
        
        public string AccountID { get; set; }
        [Key]
        public string OrderID { get; set; }
        public string InvoiceID { get; set; }
        public DateTime Date { get; set; }
        public string?  WaybillNumber { get; set; }
        public string? OrderNumber { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? VendorID { get; set; }
        public string? Vendor { get; set; }
        public string? Shop { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal OrderAmount { get; set; }
    }

}
