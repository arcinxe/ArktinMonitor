namespace ArktinMonitor.Data.Models
{
    public class BlockedApplication
    {
        public int BlockedApplicationId { get; set; }
        //public int BlockedApplicationLocalId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

        public int ComputerUserId { get; set; }
        public virtual ComputerUser ComputerUser { get; set; }
    }
}
