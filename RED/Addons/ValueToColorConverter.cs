using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Diagnostics;

namespace RED.Addons
{
    public class ValueToColorConverter : IValueConverter
    {
        /*Actuation Motor: 20-0-15-0
         *Auxiliary Motor: 15-0-10-0
         *Logic/Comms: 5-0-3-0
         *General Motor: 15-0-7-0
         *Battery Temp: 75-22-38-0
         *Pack Current: 80-0-50-0
         *Pack Voltage: 33.6-20-25-1
         *Cell Voltage: 4.2-2.5-3.1-1
         *Boolean: 1-0-0.5- (0 if 0 should be green or 1 if 0 should be red)
         */
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            String param = (String)parameter;
            String[] seperator = {"-"};
            Int32 count = 4;

            String[] bounds = param.Split(seperator, count, StringSplitOptions.RemoveEmptyEntries);

            List<double> bound = bounds.Select(x => double.Parse(x)).ToList();

            double max = bound[0], min = bound[1], cutoff = bound[2], down = bound[3];
            float val = (float)value;

            byte intensity;
            
            if (val > max && down == 1 || val < min && down == 0)
            {
                return new SolidColorBrush(Color.FromRgb(0, 255, 0));
            }
            else if (val < min && down == 1 || val > min && down == 0)
            {
                return new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }
            else if (val > cutoff && down == 1 || val < cutoff && down == 0)  //a value of 0, the worst, most red scenario, should give 255, 0, 0, the most red color
            {                       //a value of 1, the "best", least great color, should give 255, 255, 255, which is white.
                //Debug.WriteLine("condition 1");
                if (down == 0)
                {
                    intensity = (byte)((255 / (cutoff - min)) * (val - min));
                }
                else
                {
                    intensity = (byte)((255 / (max - cutoff)) * (max - val));
                }
                return new SolidColorBrush(Color.FromRgb(intensity, 255, intensity));
                //return new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
            else                    //a value of 0, the best, most green scenario, should give 0, 255, 0, the most green color
            {                       //a value of 1, the "worst", least great color, should give 255, 255, 255, which is white.
                //Debug.WriteLine("condition 2");
                if (down == 0)
                {
                    intensity = (byte)((255 / (max - cutoff)) * (max - val));
                }
                else
                {
                    intensity = (byte)((255 / (cutoff - min)) * (val - min));
                }
                return new SolidColorBrush(Color.FromRgb(255, intensity, intensity));
                //return new SolidColorBrush(Color.FromRgb(0, 0, 0));

            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
