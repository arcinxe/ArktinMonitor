using System;
using System.Globalization;
using System.Windows.Data;

namespace ArktinMonitor.DesktopApp.Helpers
{
    public class IntToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = 0;
            if (value != null)
            {
                val = (int)value;
            }
            return val > 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && (bool)value ? 1 : 0;
        }
    }
}
