using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArktinMonitor.Data.Models;

namespace ArktinMonitor.Data.ViewModels
{
    public class ComputerViewModel
    {
        public string Name { get; set; }

        public string Cpu { get; set; }

        public string Gpu { get; set; }

        public double Ram { get; set; }

        public string OperatingSystem { get; set; }

        public string MacAddress { get; set; }

        public List<Disk> Disks { get; set; }

        public List<ComputerUser> ComputerUsers { get; set; }

        public List<LogTimeInterval> LogTimeIntervals { get; set; }
    }
}
