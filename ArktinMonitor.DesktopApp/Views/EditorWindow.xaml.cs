using System;
using System.Windows;
using System.Windows.Input;
using ArktinMonitor.DesktopApp.ViewModel;

namespace ArktinMonitor.DesktopApp.Views
{
    /// <summary>
    /// Interaction logic for EditorWindow.xaml
    /// </summary>
    public partial class EditorWindow : Window
    {
        public EditorWindow()
        {
            var viewModel = new EditorViewModel();
            this.DataContext = viewModel;
            viewModel.ClosingRequest += (sender, e) => Close();
            InitializeComponent();
        }
    }
}
