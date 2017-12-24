namespace ArktinMonitor.Data.Models
{
    public class BlockedSite : BasicBlockedSite
    {
        public int BlockedSiteId { get; set; }

        public int ComputerUserId { get; set; }

        public virtual ComputerUser ComputerUser { get; set; }
    }

    public class BlockedSiteResource : BasicBlockedSite
    {
        public int BlockedSiteId { get; set; }

        public int ComputerUserId { get; set; }
    }
    public class BlockedSiteLocal : BasicBlockedSite
    {
        public int BlockedSiteId { get; set; }
    }

    public abstract class BasicBlockedSite
    {
        public string Name { get; set; }

        public string UrlAddress { get; set; }

        public bool Active { get; set; }
    }
}