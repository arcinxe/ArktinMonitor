using MaterialDesignThemes.Wpf;
using System.Windows;
using ArktinMonitor.DesktopApp.ViewModels;

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
            this.DataContext = viewModel;
            viewModel.ClosingRequest += (sender, e) => Close();
            //this.DataContext.ClosingRequest += (sender, e) => Close();
            //(EditorViewModel)this.DataContext.ClosingRequest
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            new PaletteHelper().SetLightDark(ThemeToggle.IsChecked != null && ThemeToggle.IsChecked.Value);
        }
    }
}