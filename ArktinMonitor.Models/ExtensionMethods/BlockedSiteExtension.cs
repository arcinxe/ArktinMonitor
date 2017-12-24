using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArktinMonitor.Data.Models;

namespace ArktinMonitor.Data.ExtensionMethods
{
    public static class BlockedSiteExtension
    {
        public static BlockedSiteResource ToResource(this BlockedSite blockedSite)
        {
            return new BlockedSiteResource()
            {
                Name = blockedSite.Name,
                ComputerUserId = blockedSite.ComputerUserId,
                Active = blockedSite.Active,
                BlockedSiteId = blockedSite.BlockedSiteId,
                UrlAddress = blockedSite.UrlAddress
            };
        }

        public static BlockedSiteLocal ToLocal(this BlockedSiteResource blockedSite)
        {
            return new BlockedSiteLocal()
            {
                Name = blockedSite.Name,
                Active = blockedSite.Active,
                BlockedSiteId = blockedSite.BlockedSiteId,
                UrlAddress = blockedSite.UrlAddress
            };
        }
    }
}
