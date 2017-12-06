using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArktinMonitor.Data.Models;

namespace ArktinMonitor.Data.ExtensionMethods
{
    public static class BlockedAppExtension
    {
        public static BlockedAppResource ToResource(this BlockedAppLocal app, int userId)
        {
            return new BlockedAppResource
            {
                Active = app.Active,
                BlockedAppId = app.BlockedAppId,
                Name = app.Name,
                Path = app.Path,
                ComputerUserId = userId
            };
        }

        public static BlockedAppResource ToResource(this BlockedApp app, int userId)
        {
            return new BlockedAppResource
            {
                Active = app.Active,
                BlockedAppId = app.BlockedAppId,
                Name = app.Name,
                Path = app.Path,
                ComputerUserId = userId
            };
        }

        public static BlockedApp ToModel(this BlockedAppResource app)
        {
            return new BlockedApp
            {
                Active = app.Active,
                Name = app.Name,
                Path = app.Path,
                ComputerUserId = app.ComputerUserId,
                BlockedAppId = app.BlockedAppId
            };
        }
    }
}
