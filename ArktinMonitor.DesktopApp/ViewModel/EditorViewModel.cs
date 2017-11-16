using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using ArktinMonitor.Data.Models;
using ArktinMonitor.Helpers;
using Microsoft.Win32;
using ArktinMonitor.Data.ExtensionMethods;

namespace ArktinMonitor.DesktopApp.ViewModel
{
    internal class EditorViewModel : ViewModelTemplate
    {
        //private List<BlockedApplicationLocal> _blockedAppllications;

        //public List<BlockedApplicationLocal> BlockedApplications
        //{
        //    get { return _blockedAppllications; }
        //    set
        //    {
        //        _blockedAppllications = value;
        //        RaisePropertyChangedEvent(nameof(BlockedApplications));
        //    }
        //}

        private BlockedApplicationLocal _blockedApplication;

        public BlockedApplicationLocal BlockedApplication
        {
            get { return _blockedApplication; }
            set
            {
                _blockedApplication = value;
                RaisePropertyChangedEvent(nameof(BlockedApplication));
            }
        }
        //private ObservableCollection<ComputerUserDesktop> _users = LoadUsers();

        public ObservableCollection<ComputerUserDesktop> Users { get; set; } = LoadUsers();
        //public ObservableCollection<ComputerUserDesktop> Users
        //{
        //    get { return _users; }
        //    set
        //    {
        //        _users = value;
        //        RaisePropertyChangedEvent(nameof(Users));
        //    }
        //}

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




   
        public ICommand Test2Command => new DelegateCommand(Test2);
        public void Test2()
        {
            var db = JsonLocalDatabase.Instance;
            var computer = db.Computer;
            var users = db.Computer.ComputerUsers.Select(cu => cu.ToDesktopModel());
            Users = new ObservableCollection<ComputerUserDesktop>(users);
            Users.CollectionChanged += Users_CollectionChanged;
        }

        private void Users_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        void Users_CollectionChanged(object sender, CollectionChangeEventArgs e)
        {
            
        }

        public ICommand TestCommand => new DelegateCommand(Test);
        public void Test()
        {
            User.BlockedApplications.Add(new BlockedApplicationDesktop(){Active = false, BlockedApplicationId =7, Name = "test00", Path = "working?"});
            if (BlockedApplication == null) return;
            var dialog = new OpenFileDialog
            {
                Multiselect = false,
                DefaultExt = ".exe",
                Filter = "Executables (.exe)|*.exe"
            };
            var dlg = dialog.ShowDialog();
            if (dlg == true)
            {
                BlockedApplication.Path = dialog.FileName;
            }
            LocalLogger.Log(BlockedApplication.Path);
            //LocalLogger.Log("begin");
            //if (User.BlockedApplications == null) return;
            //foreach (var blockedAppllication in User.BlockedApplications)
            //{
            //    LocalLogger.Log(blockedAppllication.Path);
            //}
            //LocalLogger.Log("end");
        }




        public static ObservableCollection<ComputerUserDesktop> LoadUsers()
        {
            var db = JsonLocalDatabase.Instance;
            var computer = db.Computer;
            var users = db.Computer.ComputerUsers.Select(cu => cu.ToDesktopModel());
                
            //var users = computer.ComputerUsers
            //    .Where(cu => !cu.Removed)
            //    .Select(u => new ComputerUserDesktop { FullName = u.FullName, Name = u.Name, Enabled = !u.Hidden, Removed = u.Removed, BlockedApplications = u.BlockedApplications }).ToList();

            //users.ForEach(u => u.VisibleName = string.IsNullOrWhiteSpace(u.FullName) ? u.Name : u.FullName);
            return new ObservableCollection<ComputerUserDesktop>(users);
        }
    }
}
