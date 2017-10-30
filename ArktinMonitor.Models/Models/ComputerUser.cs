namespace ArktinMonitor.Data.Models
{
    public class ComputerUser
    {
        public int ComputerUserId { get; set; }
        //public int ComputerUserLocalId { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string PrivilegeLevel { get; set; }
        public bool Hidden { get; set; }

        public int ComputerId { get; set; }
        public virtual Computer Computer { get; set; }
    }
}
