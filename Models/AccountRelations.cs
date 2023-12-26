namespace Marlin.sqlite.Models
{
    public class AccountRelations
    {
        
        public int Id { get; set; }
        public string Account { get; set; }
        public string ConnectedAccount { get; set; }
        public bool RequestSent { get; set; }
        public bool Approved { get; set; }
    }
}
