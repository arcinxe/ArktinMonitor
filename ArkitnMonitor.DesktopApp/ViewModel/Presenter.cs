using System.Threading.Tasks;
using System.Windows.Input;
using ArkitnMonitor.DesktopApp.Helpers;
using ArktinMonitor.Helpers;


namespace ArkitnMonitor.DesktopApp.ViewModel
{
    internal class Presenter : ObservableObject
    {
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

        public int WindowWidth { get; set; } = 300;

        public ICommand SignInCommand => new DelegateCommand(SignIn);

        public async void SignIn()
        {
            Busy = true;
            LocalLogger.Log("Signing in...");
            await Task.Run(() => Authorization.RenewBearerToken(Settings.ApiUrl, _email, _password));
            AuthorizationStatus = string.IsNullOrWhiteSpace(Authorization.GetBearerToken.AccessToken) 
                ? "Wrong email or password" : "Succes";
            WindowWidth = string.IsNullOrWhiteSpace(Authorization.GetBearerToken.AccessToken)
                ? 300
                : 420;
            RaisePropertyChangedEvent(nameof(WindowWidth));
            Busy = false;
        }

        public ICommand SignUpCommand => new DelegateCommand(SignUp);

        public void SignUp()
        {
            HyperlinkHelper.OpenUrl(Settings.ApiUrl);
        }
    }
}
