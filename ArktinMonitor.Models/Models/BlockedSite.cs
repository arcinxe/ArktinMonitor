namespace ArktinMonitor.Data.Models
{
    public class BlockedSite
    {
        public int BlockedSiteId { get; set; }
        public string Name { get; set; }
        public string UrlAddress { get; set; }

        public int ComputerUserId { get; set; }
        public virtual ComputerUser ComputerUser { get; set; }
    }
}
