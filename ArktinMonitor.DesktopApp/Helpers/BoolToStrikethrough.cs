﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ArktinMonitor.DesktopApp.Helpers
{
    internal class BoolToStrikethrough : IValueConverter
    {
        public object Convert(object value, Type targetType, object para, CultureInfo culture)
        {
            if (value != null && (bool)value)
                return TextDecorations.Strikethrough;
            return null;
        }

        public object ConvertBack(object value, Type targetType, object para, CultureInfo culture)
        {
            return new NotImplementedException();
        }
    }
}