using System;
using System.Collections.ObjectModel;
using System.Linq;
using ArktinMonitor.Data.Models;

namespace ArktinMonitor.Data.ExtensionMethods
{
    public static class ComputerUserExtension
    {
        public static ComputerUserDesktop ToDesktopModel(this ComputerUserLocal computerUser)
        {
            var blockedApps =
                computerUser.BlockedApplications.Select(
                    ba =>
                        new BlockedApplicationDesktop
                        {
                            Active = ba.Active,
                            Name = ba.Name,
                            BlockedApplicationId = ba.BlockedApplicationId,
                            FilePath = ba.Path,
                            TempFilePath = ba.Path
                        });
            return new ComputerUserDesktop
            {
                Enabled = computerUser.Enabled,
                Name = computerUser.Name,
                FullName = computerUser.FullName,
                PrivilegeLevel = computerUser.PrivilegeLevel,
                VisibleName = computerUser.FullName == string.Empty ? computerUser.Name:computerUser.FullName,
                Removed = computerUser.Removed,
                BlockedApplications = new ObservableCollection<BlockedApplicationDesktop>(blockedApps)
            };
        }


        public static ComputerUserLocal ToLocalModel(this ComputerUserDesktop computerUser)
        {
            var blockedApps =
                computerUser.BlockedApplications.Select(
                    ba =>
                        new BlockedApplicationLocal()
                        {
                            Active = ba.Active,
                            Name = ba.Name,
                            BlockedApplicationId = ba.BlockedApplicationId,
                            Path = ba.FilePath
                        });
            return new ComputerUserLocal()
            {
                Enabled = computerUser.Enabled,
                Name = computerUser.Name,
                FullName = computerUser.FullName,
                PrivilegeLevel = computerUser.PrivilegeLevel,
                Removed = computerUser.Removed,
                BlockedApplications = blockedApps.ToList()
            };
        }
    }
}
