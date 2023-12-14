using System.ComponentModel.DataAnnotations;

namespace Marlin.sqlite.Models
{
    public class UserInfo
    {
        
        public string AccountID { get; set; }
        [Key]
        public int UserID { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string PositionInCompany { get; set; }
        public bool IsRetail { get; set; }
        
    }

}
