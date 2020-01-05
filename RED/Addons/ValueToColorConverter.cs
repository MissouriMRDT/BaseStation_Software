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
            //In order to pass in several numbers as a parameter, the best way is to pass in a string
            //and to parse out the numbers. So, our numbers will be formated max-min-cutoff-down.

            String param = (String)parameter;   //So cast the parameter as a string
            String[] seperator = {"-"};         //Noting the seperator to be a -
            Int32 count = 4;                    //And that 4 numbers will be recieved

            //The next line will split the string at the - up to 4 times, and will return a list of the resulting strings
            String[] bounds = param.Split(seperator, count, StringSplitOptions.RemoveEmptyEntries);

            //We then convert each of those strings into a double, and assign them to an appropriate variable
            double max = double.Parse(bounds[0]), min = double.Parse(bounds[1]), cutoff = double.Parse(bounds[2]), down = double.Parse(bounds[3]);

            //This is the value coming in that the color should be based off of
            float val = (float)value;

            //Intensity is the color value (out of 255) that is changing
            byte intensity;
            

            //If we are above the maximum and that is our "good" value,
            //or if we are below the minimum and that is our "good" value,
            //return Green
            if (val > max && down == 1 || val < min && down == 0)
            {
                return new SolidColorBrush(Color.FromRgb(0, 255, 0));
            }
            //Similarly, if we are below the minimum and max is our good value,
            //or if we are above the maximum and min is our good value,
            //return Red
            else if (val < min && down == 1 || val > min && down == 0)
            {
                return new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }
            //If we are in between
            //and we are above the cutoff and the max is our good value,
            //or we are below the cutoff and the min is our good value,
            //then we want to be somewhere in the Green zone
            else if (val > cutoff && down == 1 || val < cutoff && down == 0)
            {                    
                //These formulas will return 255 when at the cutoff, and 0 when at the min or max
                //(at max if max is good and at min if min is good)
                //255 will make white, and 0 will make green
                if (down == 0)
                {
                    intensity = (byte)((255 / (cutoff - min)) * (val - min));
                }
                else
                {
                    intensity = (byte)((255 / (max - cutoff)) * (max - val));
                }
                return new SolidColorBrush(Color.FromRgb(intensity, 255, intensity));
            }
            //Otherwise, that means we are below the cutoff and max is the good,
            //or we are above the cutoff and min is good,
            //and either way we are in the Red zone
            else
            {
                //Again, these formulas will return 255 when at the cutoff, and 0 when at the min or max
                //(at min if max is good and at max if min is good)
                //255 will make white, and 0 will make red
                if (down == 0)
                {
                    intensity = (byte)((255 / (max - cutoff)) * (max - val));
                }
                else
                {
                    intensity = (byte)((255 / (cutoff - min)) * (val - min));
                }
                return new SolidColorBrush(Color.FromRgb(255, intensity, intensity));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
