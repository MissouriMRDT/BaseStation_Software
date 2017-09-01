using System;
using System.Text;
using System.Windows.Data;

namespace RED.Addons.Tools
{
    public class TimestampDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || !(value is DateTime))
                return "null";

            var message = new StringBuilder();
            var timestamp = (DateTime)value;

            if (timestamp == DateTime.MinValue || timestamp == DateTime.MaxValue)
                return "Never";

            var now = DateTime.Now;
            TimeSpan delta = now - timestamp;
            TimeSpan magnitude = delta.Duration();

            if (magnitude.TotalSeconds < 3)
                return "now";
            else if (magnitude.TotalMinutes < 1)
                message.AppendFormat("{0:F0}s", magnitude.TotalSeconds);
            else if (magnitude.TotalHours < 1)
                message.AppendFormat("{0:F0}m", magnitude.TotalMinutes);
            else if (magnitude.TotalDays < 1)
                message.AppendFormat("{0:F0}h", magnitude.TotalHours);
            else
                message.AppendFormat("{0:F0}d", magnitude.TotalDays);

            if (delta > TimeSpan.Zero)
                message.Append(" ago");
            else
                message.Append(" from now");

            return message.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
