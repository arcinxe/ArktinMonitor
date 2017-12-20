using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ArktinMonitor.Data.Models;

namespace ArktinMonitor.WebApp.ViewModels
{
    public class ComputerUsersViewModel
    {
        public List<ComputerUser> Users { get; set; }

        public string ComputerName { get; set; }
    }
}