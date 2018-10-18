using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Label = System.Windows.Controls.Label;

namespace RED.Addons
{
    public class StyleFromCodeBehind
    {
        public object Styling(object[] value, Type targetType, object parameter, CultureInfo culture)
        {

            Style coded = new Style(typeof(Label));
            Setter backgroundColor = new Setter()
            {
                Property = Control.BackgroundProperty,
                Value = "{Binding Path=Motor1Current, Converter={StaticResource AmpConverter}, Mode=Default}"
            };

            coded.Setters.Add(backgroundColor);
            return coded;
        }
    }
}
