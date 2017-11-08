namespace ArktinMonitor.Data.Models
{
    public class BlockedApplication : BasicBlockedApplication
    {
        public int BlockedApplicationId { get; set; }


        public int ComputerUserId { get; set; }

        public virtual ComputerUser ComputerUser { get; set; }
    }

    public class BlockedApplicationLocal : BasicBlockedApplication
    {
        public int BlockedApplicationId { get; set; }

        public bool Synced { get; set; }
    }

    public abstract class BasicBlockedApplication
    {
        public string Name { get; set; }

        public string Path { get; set; }
    }
}
