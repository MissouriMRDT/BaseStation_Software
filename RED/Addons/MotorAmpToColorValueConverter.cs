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
    public class MotorAmpToColorValueConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double max = 15, min = 0, cutoff = 7;
            float val = (float)value;

            if (val > max)
            {
                return new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }
            else if (val < min)
            {
                return new SolidColorBrush(Color.FromRgb(0, 255, 0));
            }
            else if (val > cutoff)  //a value of 0, the worst, most red scenario, should give 255, 0, 0, the most red color
            {                       //a value of 1, the "best", least great color, should give 255, 255, 255, which is white.
                byte intensity = (byte)((255 / (cutoff - min)) * (val - min));
                return new SolidColorBrush(Color.FromRgb(255, intensity, intensity));
            }
            else                    //a value of 0, the best, most green scenario, should give 0, 255, 0, the most green color
            {                       //a value of 1, the "worst", least great color, should give 255, 255, 255, which is white.
                byte intensity = (byte)((255 / (max - cutoff)) * (max - val));
                return new SolidColorBrush(Color.FromRgb(intensity, 255, intensity));
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
