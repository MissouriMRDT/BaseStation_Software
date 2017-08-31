using GMap.NET;
using GMap.NET.WindowsPresentation;
using RED.Images;
using RED.ViewModels.Navigation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RED.Addons.Navigation
{
    public class GMapMarkerCollectionMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.Length < 2) return DependencyProperty.UnsetValue;

            List<GMapMarker> markers = new List<GMapMarker>();

            if (value[0] == null) return DependencyProperty.UnsetValue;
            Waypoint currPos = (Waypoint)value[0];
            markers.Add(new GMapMarker(new PointLatLng(currPos.Latitude, currPos.Longitude))
            {
                Shape = new Path()
                {
                    Width = 32,
                    Height = 32,
                    Stretch = Stretch.Uniform,
                    Fill = new SolidColorBrush(currPos.Color),
                    Data = Geometry.Parse(ModernUIIcons.AppbarLocationCircle32),
                    ToolTip = currPos.Name
                },
                Offset = new System.Windows.Point(-16, -16),
                ZIndex = Int32.MaxValue
            });

            foreach (var coord in (IEnumerable<Waypoint>)(value[1]))
                markers.Add(new GMapMarker(new PointLatLng(coord.Latitude, coord.Longitude))
                {
                    Shape = new Path()
                    {
                        Width = 32,
                        Height = 32,
                        Stretch = Stretch.Uniform,
                        Fill = new SolidColorBrush(coord.Color),
                        Data = Geometry.Parse(ModernUIIcons.AppbarLocationRound32),
                        ToolTip = coord.Name
                    },
                    Offset = new System.Windows.Point(-16, -32),
                    ZIndex = Int32.MaxValue - 1
                });
            return markers;
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return new object[] { DependencyProperty.UnsetValue };

            List<Waypoint> coords = new List<Waypoint>();
            foreach (var marker in (IEnumerable<GMapMarker>)value)
                coords.Add(new Waypoint(marker.Position.Lat, marker.Position.Lng));

            Waypoint currPos = coords[0];
            coords.RemoveAt(0);
            return new object[] { currPos, coords.ToArray() };
        }
    }
}
