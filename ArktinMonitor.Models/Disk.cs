using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArktinMonitor.Models
{
    public class Disk
    {
        public int DiskId { get; set; }
        public int DiskLocalId { get; set; }
        public string Letter { get; set; }
        public string Name { get; set; }
        public double TotalSpace { get; set; }
        public double FreeSpace { get; set; }

        public int ComputerId { get; set; }
        public virtual Computer Computer { get; set; }
    }
}
