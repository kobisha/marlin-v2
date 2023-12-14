using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

public class InvoiceDetail
{
    public int Id { get; set; }
    public string InvoiceID { get; set; }
    public string? Barcode { get; set; }
    public string? Unit { get; set; }
    [Column(TypeName = "numeric")]
    public decimal? Quantity { get; set; }
    [Column(TypeName = "numeric")]
    public decimal? Price { get; set; }
    [Column(TypeName = "numeric")]
    public decimal? Amount { get; set; }

    [ForeignKey("InvoiceID")]
    public InvoiceHeader InvoiceHeader { get; set; }
}
