using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marlin.sqlite.Models
{
    public class ProductCategories
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? AccountID { get; set; }
         
        public string? CategoryID { get; set; }
        public string? ParentFolder { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }

    }
}
