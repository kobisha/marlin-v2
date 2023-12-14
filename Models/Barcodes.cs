using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Marlin.sqlite.Models
{
    public class Barcodes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public  string? AccountId { get; set; }
        public  string? ProductID { get; set; }
        public  string? Barcode { get; set; }
    }
}
