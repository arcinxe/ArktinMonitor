using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ArkitnMonitor.DesktopApp.Helpers;
using ArktinMonitor.Data.Models;
using ArktinMonitor.Helpers;


namespace ArkitnMonitor.DesktopApp.ViewModel
{
    internal class EditorViewModel : ViewModelTemplate
    {
        private List<BlockedApplicationLocal> _blockedAppllications;

        public List<BlockedApplicationLocal> BlockedApplications
        {
            get { return _blockedAppllications; }
            set
            {
                _blockedAppllications = value;
                RaisePropertyChangedEvent(nameof(BlockedApplications));
            }
        }

        private BlockedApplicationLocal _blockedAppllication;

        public BlockedApplicationLocal BlockedApplication
        {
            get { return _blockedAppllication; }
            set
            {
                _blockedAppllication = value;
                RaisePropertyChangedEvent(nameof(BlockedApplication));
            }
        }
        private List<ComputerUserDesktop> _users = LoadUsers();

        public List<ComputerUserDesktop> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                RaisePropertyChangedEvent(nameof(Users));
            }
        }

        private ComputerUserDesktop _user;

        public ComputerUserDesktop User
        {
            get { return _user; }
            set
            {
                _user = value;
                RaisePropertyChangedEvent(nameof(User));
            }
        }
        private string _password = "[REDACTED]";
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChangedEvent(nameof(Password));
            }
        }

        private bool _busy;
        public bool Busy
        {
            get { return _busy; }
            set
            {
                _busy = value;
                RaisePropertyChangedEvent(nameof(Busy));
            }
        }

        private string _authorizationStatus;
        public string AuthorizationStatus
        {
            get { return _authorizationStatus; }
            set
            {
                _authorizationStatus = value;
                RaisePropertyChangedEvent(nameof(AuthorizationStatus));
            }
        }

        public int WindowWidth { get; set; } = 300;




        public ICommand SignUpCommand => new DelegateCommand(SignUp);

        public void SignUp()
        {
            //HyperlinkHelper.OpenUrl(Settings.ApiUrl);
            //var newWindow = new Window1();
            //newWindow.Show();

            //var computerUsers = new List<ComputerUserDesktop>()
            //{
            //    new ComputerUserDesktop(){Name = "Marcin", FullName = "Arcin"},
            //    new ComputerUserDesktop(){Name = "Alek",FullName = "Chenio jr"},
            //    new ComputerUserDesktop(){Name = "Miłoszek"},
            //    new ComputerUserDesktop(){Name = "Andrew"},
            //    new ComputerUserDesktop(){Name = "User",FullName = "User"},
            //    new ComputerUserDesktop(){Name = "Guest"}
            //};
            //Email = computerUsers;

            //Users = new List<string>(){
            //    "Arcin",
            //    "Chenio",
            //    "Miłoszek",
            //    "Andrew",
            //    "User",
            //    "Guest"
            //};
        }

        public static List<ComputerUserDesktop> LoadUsers()
        {
            var db = JsonLocalDatabase.Instance;
            var computer = db.Computer;
            var users = computer.ComputerUsers
                .Where(cu => cu.Removed == false)
                .Select(u => new ComputerUserDesktop { FullName = u.FullName, Name = u.Name, Enabled = !u.Hidden, Removed = u.Removed, BlockedApplications = u.BlockedApplications}).ToList();
            //var users = new List<ComputerUserDesktop>()
            //{
            //    new ComputerUserDesktop(){Name = "Marcin", FullName = "Arcin"},
            //    new ComputerUserDesktop(){Name = "Alek",FullName = "Chenio jr"},
            //    new ComputerUserDesktop(){Name = "Miłoszek"},
            //    new ComputerUserDesktop(){Name = "Andrew"},
            //    new ComputerUserDesktop(){Name = "User",FullName = "User"},
            //    new ComputerUserDesktop(){Name = "Guest"}
            //};
            users.ForEach(u => u.VisibleName = string.IsNullOrWhiteSpace(u.FullName) ? u.Name : u.FullName);      
            return users;
        }
    }
}
