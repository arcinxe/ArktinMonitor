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
            InitializeComponent();
            var viewModel = new EditorViewModel();
            DataContext = viewModel;
            InitializeComponent();
            viewModel.ClosingRequest += (sender, e) => Close();
        }
    }
}
