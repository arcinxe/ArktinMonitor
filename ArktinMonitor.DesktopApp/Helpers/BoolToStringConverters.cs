﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace ArktinMonitor.DesktopApp.Helpers
{
    public class BoolToDayTimeIcon : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && (bool)value ? "Brightness3" : "Brightness5";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && (bool)value;
        }
    }
}