using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marlin.sqlite.Models
{
    public class ProductsByCategories
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? AccountID { get; set; }
        public string? ProductID { get; set; }
        public string? CategoryID { get; set; }
    }
}
