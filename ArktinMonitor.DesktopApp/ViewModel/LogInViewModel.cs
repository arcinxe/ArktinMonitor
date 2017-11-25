using ArktinMonitor.DesktopApp.Views;
using ArktinMonitor.Helpers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ArktinMonitor.DesktopApp.ViewModel
{
    internal class LogInViewModel : ViewModelTemplate
    {
        public Visibility WindowVisibility { get; set; } = Visibility.Visible;
        private string _email = "marcinxe@gmail.com";

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                RaisePropertyChangedEvent(nameof(Email));
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

        public ICommand SignInCommand => new DelegateCommand(SignIn);

        public async void SignIn()
        {
            Busy = true;
            LocalLogger.Log("Signing in...");
            var t = await Task.Run(() => CredentialsManager.RenewBearerToken(Settings.ApiUrl, _email, _password));
            _succeded = !string.IsNullOrWhiteSpace(t?.AccessToken);
            AuthorizationStatus = _succeded ? "Succes" : "Wrong email or password";
            if (_succeded)
            {
                CredentialsManager.StoreCredentials(Settings.DataStoragePath, _email, _password);
                CredentialsManager.GetJsonWebToken(Settings.ApiUrl);
                TransferToEditorWindow();
            }
            Busy = false;
        }

        public ICommand SignUpCommand => new DelegateCommand(SignUp);

        // Opens Registration page
        private void SignUp()
        {
            //HyperlinkHelper.OpenUrl($"{Settings.ApiUrl}Account/Register");
            //CredentialsManager.PurgePassword();
            AuthorizationStatus = CredentialsManager.AreCredentialsStored().ToString();
            CredentialsManager.GetJsonWebToken(Settings.ApiUrl);
        }

        private void TransferToEditorWindow()
        {
            var window = new EditorWindow();
            window.Show();
            OnClosingRequest();
        }
    }
}