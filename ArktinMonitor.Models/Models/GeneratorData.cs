using System.Collections.Generic;

namespace ArktinMonitor.Data.Models
{
    public class GeneratorData
    {
        public List<string> Manufacturers { get; set; }

        public List<string> Cpus { get; set; }

        public List<string> Gpus { get; set; }

        public List<string> UserNames { get; set; }

        public List<string> DiskNames { get; set; }

        public List<string> OperatingSystems { get; set; }

        public List<string> BlockedAppsPaths { get; set; }

        public List<string> BlockedSites { get; set; }
    }
}