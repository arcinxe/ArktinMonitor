using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ArktinMonitor.Data.Models
{
    public class Credentials
    {
        public SecureString Token { get; set; }

        public string Email { get; set; }
    }
}
