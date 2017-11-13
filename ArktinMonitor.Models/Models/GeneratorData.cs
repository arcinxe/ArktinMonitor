using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArktinMonitor.Data.Models
{
    class GeneratorData
    {
        public List<string> ComputerBrands { get; set; }

        public List<string> Cpus { get; set; }

        public List<string> Gpus { get; set; }

        public List<string> UserNames { get; set; }

        public List<string> DiskNames { get; set; }

        public List<string> OperatingSystems { get; set; }

        public List<string> BlockedAppsPaths { get; set; }

        public List<string> BlockedSites { get; set; }
    }
}
