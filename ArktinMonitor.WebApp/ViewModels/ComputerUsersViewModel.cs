using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ArktinMonitor.Data.Models;

namespace ArktinMonitor.WebApp.ViewModels
{
    public class ComputerUsersViewModel
    {
        public List<ComputerUserViewModel> Users { get; set; }

        public string ComputerName { get; set; }

        public int ComputerId { get; set; }
    }

    public class ComputerUserViewModel
    {
        public ComputerUser Details { get; set; }

        public IEnumerable<BlockedApp> BlockedApps { get; set; }

        public IEnumerable<BlockedSite> BlockedSites { get; set; }
    }

    //public static class ComputerUsersViewModelExtension
    //{
    //    public static ComputerUserViewModel ToViewModel(this IEnumerable<ComputerUser> users,
    //        IEnumerable<BlockedApp> blockedApps, IEnumerable<BlockedSite> blockedSites)
    //    {
    //        return new ComputerUserViewModel
    //        {
                
    //        };
    //    }
    //}
}