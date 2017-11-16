using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                            Path = ba.Path
                        });
            return new ComputerUserDesktop
            {
                Enabled = !computerUser.Hidden,
                Name = computerUser.Name,
                FullName = computerUser.FullName,
                PrivilegeLevel = computerUser.PrivilegeLevel,
                VisibleName = computerUser.Name,
                BlockedApplications = new ObservableCollection<BlockedApplicationDesktop>(blockedApps)
            };
        }
    }
}
