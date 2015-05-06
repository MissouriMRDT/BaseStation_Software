using GMap.NET;
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RED.Addons
{
    public class GPSCoordinateListToGMapMarkerListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return DependencyProperty.UnsetValue;
            List<GMapMarker> markers = new List<GMapMarker>();
            foreach (var coord in (IEnumerable<GPSCoordinate>)value)
            {
                markers.Add(new GMapMarker(new PointLatLng(coord.Longitude, coord.Latitude))
                {
                    Shape = new Path()
                    {
                        Width = 32,
                        Height = 32,
                        Stretch = Stretch.Fill,
                        Fill = Brushes.Red,
                        Data = Geometry.Parse(RED.Images.ModernUIIcons.AppbarLocationRound32)
                    },
                    Offset = new System.Windows.Point(-16, -16),
                    ZIndex = int.MaxValue
                });
            }
            return markers;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return DependencyProperty.UnsetValue;
            List<GPSCoordinate> coords = new List<GPSCoordinate>();
            foreach (var marker in (IEnumerable<GMapMarker>)value)
                coords.Add(new GPSCoordinate(marker.Position.Lat, marker.Position.Lng));
            return coords;
        }
    }
}
