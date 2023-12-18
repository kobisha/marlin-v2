﻿using System.ComponentModel.DataAnnotations;

namespace Marlin.sqlite.Models
{
    public class RetroBonusResultFront
    {
       
        public string RetroBonusID { get; set; }
        [Key]
        public string Barcode { get; set; }
        
        public string? Product { get; set; }
        public decimal? RetroPercent { get; set; }
        public decimal? PurchaseAmount { get; set; }
        public decimal? Stock { get; set; }
    }

}
