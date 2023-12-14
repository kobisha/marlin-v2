using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Marlin.sqlite.Models
{
    public class StocksOfProducts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? AccountID { get; set; }
        public string? Barcode { get; set; }
        public string? ShopID { get; set; }
        public decimal?   Quantity { get; set; }
    }
}
