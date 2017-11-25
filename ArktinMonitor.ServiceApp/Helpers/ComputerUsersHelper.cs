using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Security.Principal;
using ArktinMonitor.Data.Models;
using ArktinMonitor.Helpers;
using ArktinMonitor.ServiceApp.Services;

namespace ArktinMonitor.ServiceApp.Helpers
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

        public static List<ComputerUserLocal> GetComputerUsers()
        {
            var userGroups = LoadUserGroups();
            var accounts = new List<ComputerUserLocal>();
            var query = new SelectQuery("Win32_UserAccount");
            var searcher = new ManagementObjectSearcher(query);
            foreach (var managementObject in searcher.Get())
            {
                var account = (ManagementObject)managementObject;
                var isAccountRight = userGroups.Any(g => g.User == account.GetPropertyValue("Name").ToString()) &&
                                      !(bool)account.GetPropertyValue("disabled") &&
                                      !(bool)account.GetPropertyValue("lockout") &&
                                      (bool)account.GetPropertyValue("LocalAccount");
                if (isAccountRight)
                {
                    accounts.Add(new ComputerUserLocal()
                    {
                        FullName = account.GetPropertyValue("FullName").ToString(),
                        Name = account.GetPropertyValue("Name").ToString(),
                        PrivilegeLevel = userGroups.Where(g => g.User == account.GetPropertyValue("Name").ToString()).Any(g => g.Group == AdministratorsGroupName)
                        ? "Administrator" : "Standard user",
                        BlockedSites = new List<BlockedSiteLocal>(),
                        BlockedApplications = new List<BlockedApplicationLocal>()
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
            foreach (var managementObject in searcher.Get())
            {
                var userGroup = (ManagementObject)managementObject;
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

        /// <summary>
        /// Returns currently logged in username or null if no one is logged in
        /// </summary>
        /// <returns>Username if any user is logged in, null otherwise</returns>
        public static string CurrentlyLoggedInUser()
        {
            try
            {
                var searcher = new ManagementObjectSearcher("SELECT UserName FROM Win32_ComputerSystem");
                var collection = searcher.Get();
                var username = (string)collection.Cast<ManagementBaseObject>().First()["UserName"];
                return username.Split('\\')[1];
            }
            catch (Exception)
            {
                //LocalLogger.Log("CurrentlyLoggedInUser", e);
                return null;
            }
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
