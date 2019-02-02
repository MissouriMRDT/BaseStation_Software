using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace RoverNetworkManager.Addons
{
    class PingColorValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int max = 15, min = 0, cutoff = 5, val;
            
            val = ToInt32(value);

            if (val > max || val < min)
            {
                return new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }
            else if (val > cutoff)  //a value of 0, the worst, most red scenario, should give 255, 0, 0, the most red color
            {                       //a value of 1, the "best", least great color, should give 255, 255, 255, which is white.
                byte intensity = (byte)((255 / (max - cutoff)) * (max - val));
                return new SolidColorBrush(Color.FromRgb(255, 255, intensity));
            }
            else                    //a value of 0, the best, most green scenario, should give 0, 255, 0, the most green color
            {                       //a value of 1, the "worst", least great color, should give 255, 255, 255, which is white.
                byte intensity = (byte)((255 / (cutoff - min)) * (val - min));
                return new SolidColorBrush(Color.FromRgb(intensity, 255, intensity));
            }
        }

        private int ToInt32(object value)
        {
            return value == null ? 0 : ((IConvertible)value).ToInt32(null);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
