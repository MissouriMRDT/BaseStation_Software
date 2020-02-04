﻿using System;
using System.Windows.Data;

namespace RED.Addons.Tools
{
    public class TimeSpanToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return String.Empty;

            var val = (TimeSpan)value;
            val = TimeSpan.FromSeconds(Math.Round(val.TotalSeconds));
            return String.Format("{0}{1:#00}:{2:00}", val.Seconds >= 0 ? "" : "-", Math.Abs(Math.Truncate(val.TotalMinutes)), Math.Abs(val.Seconds));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
