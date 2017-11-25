using System;
using System.Windows.Input;

namespace ArktinMonitor.DesktopApp.ViewModel
{
    internal class DelegateCommand : ICommand
    {
        private readonly Action _action;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action();
        }

        public DelegateCommand(Action action)
        {
            _action = action;
        }

#pragma warning disable 67

        public event EventHandler CanExecuteChanged;

#pragma warning restore 67
    }
}