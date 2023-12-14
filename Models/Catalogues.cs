using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marlin.sqlite.Models
{
    public class Catalogues
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? AccountID { get; set; }
         
        public string ProductID { get; set; } 
        public string? SourceCode { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        
        public string? Unit { get; set; }
        public string? Status { get; set; }
        
        public DateTime LastChangeDate { get ; set ; }

        
    }

    public class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        }
    }
}
