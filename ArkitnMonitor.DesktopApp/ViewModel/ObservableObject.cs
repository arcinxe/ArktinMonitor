using System.ComponentModel;

namespace ArkitnMonitor.DesktopApp.ViewModel
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        protected void RaisePropertyChangedEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            //var handler = PropertyChanged;
            //if (handler != null)
            //    handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
