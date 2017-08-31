using System;
using System.Windows.Data;

namespace RED.Addons.Navigation
{
    public class GPSCoordinateToLatitudeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((GPSCoordinate)value).Latitude;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}