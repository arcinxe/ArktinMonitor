using System;
using System.ComponentModel;

namespace ArktinMonitor.DesktopApp.ViewModel
{
    public class ViewModelTemplate : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
        protected void RaisePropertyChangedEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // https://stackoverflow.com/questions/11945821/implementing-close-window-command-with-mvvm
        public event EventHandler ClosingRequest;
        protected void OnClosingRequest()
        {
            ClosingRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}
