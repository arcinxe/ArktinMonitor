using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ArktinMonitor.DesktopApp.ViewModel;
using MaterialDesignThemes.Wpf;

namespace ArktinMonitor.DesktopApp.Views
{
    /// <summary>
    /// Interaction logic for EditorWindow.xaml
    /// </summary>
    public partial class EditorWindow : Window
    {
        public EditorWindow()
        {
            //var viewModel = new EditorViewModel();
            //this.DataContext = viewModel;
            //viewModel.ClosingRequest += (sender, e) => Close();
            //this.DataContext.ClosingRequest += (sender, e) => Close();
            //(EditorViewModel)this.DataContext.ClosingRequest
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            new PaletteHelper().SetLightDark(ThemeToggle.IsChecked != null && ThemeToggle.IsChecked.Value);
        }
    }
}
