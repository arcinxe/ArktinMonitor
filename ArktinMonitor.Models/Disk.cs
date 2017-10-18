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
        public char Letter { get; set; }
        public string Name { get; set; }
        public double TotalSpace { get; set; }
        public double UsedSpace { get; set; }

        public int ComputerId { get; set; }
        public virtual Computer Computer { get; set; }
    }
}
