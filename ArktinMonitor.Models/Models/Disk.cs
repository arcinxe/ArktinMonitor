namespace ArktinMonitor.Data.Models
{
    public class Disk : BasicDisk
    {
        public int DiskId { get; set; }
        
        public int ComputerId { get; set; }
        public virtual Computer Computer { get; set; }
    }

    public class DiskResourceModel : BasicDisk
    {
        public ComputerResourceModel Computer { get; set; }
    }

    public class BasicDisk
    {
        public string Letter { get; set; }
        public string Name { get; set; }
        public double TotalSpace { get; set; }
        public double FreeSpace { get; set; }
    }
}
