using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marlin.sqlite.Models
{
    public class AccountData
    {
        public string AccountID { get; set; }
        [Key]
        public string? LegalCode { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        [NotMapped]
        public bool IsVendor { get; set; }

        [NotMapped]
        public bool IsRetail { get; set; }

        [Column("IsVendor")]
        public int IsVendorInt
        {
            get { return IsVendor ? 1 : 0; }
            set { IsVendor = value == 1; }
        }

        [Column("IsRetail")]
        public int IsRetailInt
        {
            get { return IsRetail ? 1 : 0; }
            set { IsRetail = value == 1; }
        }
        public decimal? ProductsCount { get; set; }
    }
}
