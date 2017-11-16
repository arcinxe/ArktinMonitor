using System;
using System.Globalization;
using System.Windows.Data;

namespace ArktinMonitor.DesktopApp.Helpers
{
    public class InvertBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && (bool)value)
            {
                return false;
            }
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && (bool)value)
            {
                return false;
            }
            return true;
        }
    }
}
