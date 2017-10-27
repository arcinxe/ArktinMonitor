using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using ArktinMonitor.Data;
using ArktinMonitor.Data.Models;

namespace ArktinMonitor.ConsoleClient.Helpers
{
    class ComputerUsersHelper
    {
        // Stores the localized version of group Administrators.
        private static readonly string AdministratorsGroupName = new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null)
            .Translate(typeof(NTAccount))
            .Value.Split('\\')[1];

        // Stores the localized version of group ComputerUsers.
        private static readonly string UsersGroupName = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null)
            .Translate(typeof(NTAccount))
            .Value.Split('\\')[1];

        public static List<ComputerUser> GetComputerUsers()
        {
            var counter = 1;
            var userGroups = LoadUserGroups();
            var accounts = new List<ComputerUser>();
            var query = new SelectQuery("Win32_UserAccount");
            var searcher = new ManagementObjectSearcher(query);
            foreach (ManagementObject account in searcher.Get())
            {
                bool isAccountRight = userGroups.Any(g => g.User == account.GetPropertyValue("Name").ToString()) &&
                                      !(bool)account.GetPropertyValue("disabled") &&
                                      !(bool)account.GetPropertyValue("lockout") &&
                                      (bool)account.GetPropertyValue("LocalAccount");
                if (isAccountRight)
                {
                    accounts.Add(new ComputerUser()
                    {
                        FullName = account.GetPropertyValue("FullName").ToString(),
                        Name = account.GetPropertyValue("Name").ToString(),
                        PrivilegeLevel = userGroups.Where(g => g.User == account.GetPropertyValue("Name").ToString()).Any(g => g.Group == AdministratorsGroupName)
                        ? "Administrator" : "Standard user",
                        ComputerUserLocalId = counter++
                    });
                }
            }
            return accounts;
        }

        private static List<UserGroup> LoadUserGroups()
        {
            var userGroups = new List<UserGroup>();

            var query = new SelectQuery("Win32_GroupUser");
            var searcher = new ManagementObjectSearcher(query);
            foreach (ManagementObject userGroup in searcher.Get())
            {
                bool isAdminOrUser = (GetName(userGroup, "GroupComponent") == AdministratorsGroupName) ||
                                     (GetName(userGroup, "GroupComponent") == UsersGroupName);

                if (isAdminOrUser)
                {
                    userGroups.Add(new UserGroup()
                    {
                        Group = GetName(userGroup, "GroupComponent"),
                        User = GetName(userGroup, "PartComponent")
                    });
                }
            }
            return userGroups;

        }

        // Extracts Name property from strings like that
        // GroupComponent : \\ARCIN-NOTEBOOK\root\cimv2:Win32_Group.Domain="ARCIN-NOTEBOOK",Name="Administratorzy"
        private static string GetName(ManagementBaseObject obj, string propertyValue)
        {
            var splitName = obj.GetPropertyValue(propertyValue)
                .ToString().Split(',')[1];
            var startIndex = splitName.IndexOf('"') + 1;
            var length = splitName.LastIndexOf('"') - startIndex;
            var username = splitName.Substring(startIndex, length);
            return username;
        }

        private class UserGroup
        {
            public string User { get; set; }
            public string Group { get; set; }
        }
    }
}
