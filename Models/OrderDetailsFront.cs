using System.ComponentModel.DataAnnotations;

namespace Marlin.sqlite.Models
{
    public class OrderDetailsFront
    {
      
        
        
        public string? Product { get; set;}
        [Key] 
        public string? Barcode { get; set;}
        public string? Unit { get; set;}
        public decimal? Quantity { get; set;}
        public decimal? Price { get; set;}
        public decimal? Amount { get; set;}
        public decimal? ReservedQuantity { get; set;}
        public decimal? InvoiceQuantity { get; set;}
        public decimal? InvoiceAmount { get; set;}

        public Boolean RedStatus { get; set;}
    }
}
