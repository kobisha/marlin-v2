namespace Marlin.sqlite.Models
{
    public class ExchangeLog
    {
        public int id { get; set; }
        public string? AccountID { get; set; }
        public DateTimeOffset Date { get; set; }
        public string? JsonBody { get; set; }
        public string? Status { get; set; }
        public string? ErrorCode { get; set; }
    }
}
