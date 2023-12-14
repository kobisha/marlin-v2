using System.ComponentModel.DataAnnotations;

public class InvoiceHeader
{
    public int ID { get; set; }
    public string OrderID { get; set; }
    [Key]
    public string? InvoiceID { get; set; }
    public DateTimeOffset Date { get; set; }
    public string? Number { get; set; }
    public decimal? Amount { get; set; }
    public string? WaybillNumber { get; set; }
    public DateTimeOffset PaymentDate { get; set; }
    public List<InvoiceDetail> InvoiceDetails { get; set; }
}