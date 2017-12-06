using ArktinMonitor.DesktopApp.Views;
using ArktinMonitor.Helpers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ArktinMonitor.DesktopApp.Helpers;
using ArktinMonitor.DesktopApp.ViewModels;

namespace ArktinMonitor.DesktopApp.ViewModels
{
    internal class LogInViewModel : ViewModelTemplate
    {
        public LogInViewModel()
        {
            _credentials = new CredentialsManager(Settings.ApiUrl, Settings.UserRelatedStoragePath,
                Settings.SystemRelatedStoragePath, "ArktinMonitor");
            _email = _credentials.GetEmail();


        }
        private readonly CredentialsManager _credentials;
        public Visibility WindowVisibility { get; set; } = Visibility.Visible;
        private string _email;

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                RaisePropertyChangedEvent(nameof(Email));
            }
        }

        private string _password/* = "[REDACTED]"*/;

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChangedEvent(nameof(Password));
            }
        }

        private bool _busy = false;

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

        private bool _succeded;

        public ICommand WindowLoadedCommand => new DelegateCommand(WindowLoaded);

        private async void WindowLoaded()
        {
            if (string.IsNullOrWhiteSpace(_credentials.GetEmail())) return;
            Busy = true;
            var access = await Task.Run(() => _credentials.TryGetWebApiToken());
            if (access) TransferToEditorWindow();
            else Busy = false;
        }
        public ICommand SignInCommand => new DelegateCommand(SignIn);

        public async void SignIn()
        {
            Busy = true;
            LocalLogger.Log("Signing in...");
            var t = await Task.Run(() => _credentials.RenewBearerToken(_email, _password));
            _succeded = !string.IsNullOrWhiteSpace(t?.AccessToken);
            AuthorizationStatus = _succeded ? "Success" : "Wrong email or password";
            if (_succeded)
            {
                _credentials.StoreCredentials(_email, _password);
                _credentials.GetJsonWebToken();
                TransferToEditorWindow();
            }
            Busy = false;
        }

        public ICommand SignUpCommand => new DelegateCommand(SignUp);

        // Opens Registration page
        private void SignUp()
        {
            HyperlinkHelper.OpenUrl($"{Settings.ApiUrl}Account/Register");
            //_credentials.RemovePassword();
            //AuthorizationStatus = _credentials.AreCredentialsStored().ToString();
            //_credentials.GetJsonWebToken();
        }

        private void TransferToEditorWindow()
        {
            var window = new EditorWindow();
            window.Show();
            OnClosingRequest();
        }
    }
}