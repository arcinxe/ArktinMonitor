using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArktinMonitor.Models
{
    public class BlockedSite
    {
        public int BlockedSiteId { get; set; }
        public string Name { get; set; }
        public string UrlAddress { get; set; }

        public int ComputerUserId { get; set; }
        public virtual ComputerUser ComputerUser { get; set; }
    }
}
