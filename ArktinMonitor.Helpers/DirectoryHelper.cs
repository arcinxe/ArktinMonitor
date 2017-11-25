using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace ArktinMonitor.Helpers
{
    public static class DirectoryHelper
    {
        public static bool IsPathWritable(string path)
        {
            var permissionSet = new PermissionSet(PermissionState.None);

            var writePermission = new FileIOPermission(FileIOPermissionAccess.Write, path);

            permissionSet.AddPermission(writePermission);

            return permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet);
        }
    }
}
