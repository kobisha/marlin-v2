using System.ComponentModel.DataAnnotations;

namespace Marlin.sqlite.Models
{
    public class OrderStatusResult
    {
        public int Id { get; set; }
        public string? OrderID { get; set; }
        public DateTime Date { get; set; }
        
        public string? StatusName { get; set; }
    }
}
