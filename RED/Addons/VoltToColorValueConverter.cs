using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace RED.Addons
{
    public class VoltToColorValueConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int max = 15;
            double cutoff = 1.0;

            if((float)value > cutoff)  //a value of 0, the worst, most red scenario, should give 255, 0, 0, the most red color
            {                       //a value of 1, the "best", least great color, should give 255, 255, 255, which is white.
                byte intensity = (byte)((((float)value)-cutoff) * (255/max));
                return new SolidColorBrush(Color.FromRgb(intensity, 255, intensity));
            }
            else                    //a value of 0, the best, most green scenario, should give 0, 255, 0, the most green color
            {                       //a value of 1, the "worst", least great color, should give 255, 255, 255, which is white.

                byte intensity = (byte)((((float)value)) * (255/cutoff));
                return new SolidColorBrush(Color.FromRgb(255, intensity, intensity));

            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
