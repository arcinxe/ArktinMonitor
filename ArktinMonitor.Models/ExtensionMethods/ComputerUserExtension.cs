using System;
using System.Collections.Generic;
using ArktinMonitor.Data.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace ArktinMonitor.Data.ExtensionMethods
{
    public static class ComputerUserExtension
    {
        public static ComputerUserDesktop ToDesktopModel(this ComputerUserLocal computerUser)
        {
            var blockedApps =
                computerUser.BlockedApps?.Select(
                    ba =>
                        new BlockedAppDesktop
                        {
                            Active = ba.Active,
                            Name = ba.Name,
                            BlockedAppId = ba.BlockedAppId,
                            FilePath = ba.Path,
                            TempFilePath = ba.Path
                        });
            return new ComputerUserDesktop
            {
                Enabled = computerUser.Enabled,
                Name = computerUser.Name,
                FullName = computerUser.FullName,
                PrivilegeLevel = computerUser.PrivilegeLevel,
                VisibleName = computerUser.FullName == string.Empty ? computerUser.Name : computerUser.FullName,
                Removed = computerUser.Removed,
                BlockedApps = new ObservableCollection<BlockedAppDesktop>(blockedApps ?? new List<BlockedAppDesktop>())
            };
        }

        public static ComputerUserLocal ToLocal(this ComputerUserDesktop computerUser)
        {
            var blockedApps =
                computerUser.BlockedApps.Select(
                    ba =>
                        new BlockedAppLocal()
                        {
                            Active = ba.Active,
                            Name = ba.Name,
                            BlockedAppId = ba.BlockedAppId,
                            Path = ba.FilePath
                        });
            return new ComputerUserLocal()
            {
                Enabled = computerUser.Enabled,
                Name = computerUser.Name,
                FullName = computerUser.FullName,
                PrivilegeLevel = computerUser.PrivilegeLevel,
                Removed = computerUser.Removed,
                BlockedApps = blockedApps.ToList()
            };
        }

        public static ComputerUserResource ToResource(this ComputerUserLocal computerUser, int computerId)
        {
            return new ComputerUserResource()
            {
                Name = computerUser.Name,
                FullName = computerUser.FullName,
                PrivilegeLevel = computerUser.PrivilegeLevel,
                Removed = computerUser.Removed,
                ComputerUserId = computerUser.ComputerUserId,
                ComputerId = computerId
            };
        }

        public static ComputerUser ToModel(this ComputerUserResource computerUser)
        {
            return new ComputerUser()
            {
                Name = computerUser.Name,
                FullName = computerUser.FullName == String.Empty ? computerUser.Name : computerUser.FullName,
                PrivilegeLevel = computerUser.PrivilegeLevel,
                Removed = computerUser.Removed,
                ComputerUserId = computerUser.ComputerUserId,
                ComputerId = computerUser.ComputerId
            };
        }

        public static ComputerUserResource ToResource(this ComputerUser computerUser)
        {
            return new ComputerUserResource()
            {
                Name = computerUser.Name,
                FullName = computerUser.FullName,
                PrivilegeLevel = computerUser.PrivilegeLevel,
                Removed = computerUser.Removed,
                ComputerUserId = computerUser.ComputerUserId,
                ComputerId = computerUser.ComputerId
            };
        }


        public static ComputerUserLocal ToLocal(this ComputerUserResource computerUser)
        {
            return new ComputerUserLocal()
            {
                Name = computerUser.Name,
                FullName = computerUser.FullName,
                PrivilegeLevel = computerUser.PrivilegeLevel,
                Removed = computerUser.Removed,
                ComputerUserId = computerUser.ComputerUserId,
                Enabled = true,
                Synced = true
            };
        }
    }
}