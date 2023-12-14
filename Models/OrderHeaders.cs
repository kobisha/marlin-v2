using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marlin.sqlite.Models
{
    public class OrderHeaders
    {   public int Id { get; set; }
        public string AccountID { get; set; }
        [Key]
        public string OrderID { get; set; }
        
        public DateTime Date { get; set; }
        public string Number { get; set; }
        public string SenderID { get; set; }
        public string ReceiverID { get; set; }
        public string ShopID { get; set; }
        [Column(TypeName = "numeric")]
        public decimal? Amount { get; set; }
        [Column(TypeName = "numeric")]
        public int StatusID { get; set; }
        public int SendStatus { get; set; }
        public ICollection<OrderDetails> Products { get; set; }
    }


}
