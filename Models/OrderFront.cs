using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Marlin.sqlite.Models
{
    public class OrderFront
    {
        
        
        public string? AccountID { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? OrderID { get; set; }
        
        public string? Date { get; set; }
        public string? Number { get; set; }
        public string? vendorid { get; set; }


        public string? Vendor { get; set; }
        public string? Shop { get; set; }
        public decimal? Amount { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public string? Status { get; set; }
        public string? Scheduled { get; set; }


        

    }
}
