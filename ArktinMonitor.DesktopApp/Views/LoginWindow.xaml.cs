using ArktinMonitor.DesktopApp.ViewModels;
using System.Windows;

namespace ArktinMonitor.DesktopApp.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            var viewModel = new LogInViewModel();
            DataContext = viewModel;
            viewModel.ClosingRequest += (sender, e) => Close();
        }
    }
}