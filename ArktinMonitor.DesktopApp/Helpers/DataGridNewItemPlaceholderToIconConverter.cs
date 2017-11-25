using System;
using System.Globalization;
using System.Windows.Data;

namespace ArktinMonitor.DesktopApp.Helpers
{
    public class DataGridNewItemPlaceholderToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && value.ToString() == "{DataGrid.NewItemPlaceholder}" ? "Plus" : "Pencil";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && (bool)value;
        }
    }

    public class DataGridNewItemPlaceholderToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && value.ToString() == "{DataGrid.NewItemPlaceholder}" ? "Hidden" : "Visible";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && (bool)value;
        }
    }

    public class DataGridNewItemPlaceholderToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null || value.ToString() != "{DataGrid.NewItemPlaceholder}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && (bool)value;
        }
    }
}