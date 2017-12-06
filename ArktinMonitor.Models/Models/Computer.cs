using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArktinMonitor.Data.Models
{
    public class Computer : BasicComputer
    {
        public int ComputerId { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = true)]
        public new double Ram { get; set; }

        public int WebAccountId { get; set; }

        public virtual WebAccount WebAccount { get; set; }
    }

    public class ComputerViewModel : BasicComputer
    {
        public int ComputerId { get; set; }

        public List<Disk> Disks { get; set; }

        public List<ComputerUser> ComputerUsers { get; set; }

        public List<LogTimeIntervalViewModel> LogTimeIntervals { get; set; }
    }

    public class ComputerResourceModel : BasicComputer
    {
        public int ComputerId { get; set; }
    }

    public class ComputerLocal : BasicComputer
    {
        public int ComputerId { get; set; }

        public bool Synced { get; set; }

        public List<DiskLocal> Disks { get; set; }

        public List<ComputerUserLocal> ComputerUsers { get; set; }

        public List<LogTimeIntervalLocal> LogTimeIntervals { get; set; }
    }

    public abstract class BasicComputer
    {
        public string Name { get; set; }

        public string Cpu { get; set; }

        public string Gpu { get; set; }

        public double Ram { get; set; }

        public string OperatingSystem { get; set; }

        public string MacAddress { get; set; }
    }
}