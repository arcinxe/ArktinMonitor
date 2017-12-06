using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ArktinMonitor.Helpers
{
    public static class UacHelper
    {
        public static bool IsElevated()
        {
            var securityIdentifier = WindowsIdentity.GetCurrent().Owner;
            return securityIdentifier != null && securityIdentifier
                    .IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid);
        }
    }
}
