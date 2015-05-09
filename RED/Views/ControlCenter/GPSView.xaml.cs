using GMap.NET;
using GMap.NET.WindowsPresentation;
using GMap.NET.MapProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RED.Views.ControlCenter
{
    /// <summary>
    /// Interaction logic for GPSView.xaml
    /// </summary>
    public partial class GPSView : UserControl
    {
        // Marker
        GMapMarker currentMarker;
        public GPSView()
        {
            InitializeComponent();

            // Switch to Cache mode if internet is not avaible
            //TODO: Check if we have internet access
            MainMap.Manager.Mode = AccessMode.CacheOnly;
            // MessageBox.Show("No internet connection available, going to CacheOnly mode.", "GMap.NET - Demo.WindowsPresentation", MessageBoxButton.OK, MessageBoxImage.Warning);

            // Basic Map Config
            MainMap.MapProvider = GMapProviders.OpenStreetMapQuestHybrid;
            MainMap.Position = new PointLatLng(37.848544, -91.7715303);
            MainMap.Zoom = 10.0;

            // Set Marker
            currentMarker = new GMapMarker(MainMap.Position)
            {
                Shape = new Path()
                {
                    Width = 32,
                    Height = 32,
                    Stretch = Stretch.Fill,
                    Fill = Brushes.Red,
                    Data = Geometry.Parse(RED.Images.ModernUIIcons.AppbarLocationCircle32)
                },
                Offset = new System.Windows.Point(-16, -16),
                ZIndex = int.MaxValue
            };
            MainMap.Markers.Add(currentMarker);
        }

        // TODO: CREATE FORMAT CONVERTER FOR NORMAL COORDINATES
        private void AddWaypointBtn_Click(object sender, RoutedEventArgs e)
        {
            PlaceMarker(Double.Parse(LatitudeTextBox.Text), Double.Parse(LongitudeTextBox.Text));
        }
        private void PlaceMarker(double latitude, double longitude)
        {
            MainMap.Markers.Add(new GMapMarker(new PointLatLng(latitude, longitude))
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

        private void ImportBtn_Click(object sender, RoutedEventArgs e)
        {
            MainMap.ShowImportDialog();
        }
        private void ExportBtn_Click(object sender, RoutedEventArgs e)
        {
            MainMap.ShowExportDialog();
        }
        private void PrefetchBtn_Click(object sender, RoutedEventArgs e)
        {
            RectLatLng area = MainMap.SelectedArea;
            if (!area.IsEmpty)
            {
                for (int i = (int)MainMap.Zoom; i <= MainMap.MaxZoom; i++)
                {
                    MessageBoxResult res = MessageBox.Show("Ready ripp at Zoom = " + i + " ?", "GMap.NET", MessageBoxButton.YesNoCancel);

                    if (res == MessageBoxResult.Yes)
                    {
                        TilePrefetcher obj = new TilePrefetcher();
                        obj.Owner = Application.Current.MainWindow;
                        obj.ShowCompleteMessage = true;
                        obj.Start(area, i, MainMap.MapProvider, 100);
                    }
                    else if (res == MessageBoxResult.No)
                    {
                        continue;
                    }
                    else if (res == MessageBoxResult.Cancel)
                    {
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Select map area holding ALT", "GMap.NET", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are You sure?", "Clear GMap.NET cache?", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                try
                {
                    MainMap.Manager.PrimaryCache.DeleteOlderThan(DateTime.Now, null);
                    MessageBox.Show("Done. Cache is clear.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}