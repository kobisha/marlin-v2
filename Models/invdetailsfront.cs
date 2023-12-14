using System.ComponentModel.DataAnnotations;

namespace Marlin.sqlite.Models
{
    public class invdetails
    {
       
       
        public string? Product { get; set; }
        [Key] 
        public string Barcode { get; set; }
        public string? Unit { get; set; }
        public decimal? InvoiceQuantity { get; set; }
        public decimal? OrderQuantity { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public decimal? OrderAmount { get; set; }
        public bool RedStatus { get; set; }
    }
}
