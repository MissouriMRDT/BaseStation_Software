using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using RED.Addons;
using RED.ViewModels.Modules;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RED.Views.Modules
{
    /// <summary>
    /// Interaction logic for GPSView.xaml
    /// </summary>
    public partial class GPSView : UserControl
    {
        // Marker
        public GPSView()
        {
            InitializeComponent();

            MainMap.Manager.Mode = AccessMode.CacheOnly;
            MainMap.MapProvider = GMapProviders.OpenStreetMapQuestHybrid;
            MainMap.Position = new PointLatLng(RED.Properties.Settings.Default.GPSStartLocationLatitude, RED.Properties.Settings.Default.GPSStartLocationLongitude);
            MainMap.Zoom = 10.0;

            MainMap.EmptyMapBackground = Brushes.White;
            MainMap.EmptytileBrush = Brushes.White;
            MainMap.EmptyTileText = new FormattedText("No imagery available in cache.", System.Globalization.CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, new Typeface("Sans Serif"), SystemFonts.MessageFontSize, Brushes.Black);

            MainMap.IgnoreMarkerOnMouseWheel = true;
            MainMap.MouseWheelZoomType = MouseWheelZoomType.MousePositionWithoutCenter;
            //MainMap.FillEmptyTiles = false; //Good for debugging map cache
        }

        private void AddWaypointBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double yCoord = Utilities.ParseCoordinate(LatitudeTextBox.Text);
                double xCoord = Utilities.ParseCoordinate(LongitudeTextBox.Text);
                ((GPSViewModel)DataContext).Waypoints.Add(new Addons.GPSCoordinate(yCoord, xCoord));
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Invalid Longitude or Latitude. Must be a floating point number.");
            }
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

        private void RefreshMap_Click(object sender, RoutedEventArgs e)
        {
            var converter = new RED.Addons.GMapMarkerCollectionMultiConverter();
            var vm = (GPSViewModel)DataContext;
            MainMap.Markers.Clear();
            var newdata = (IEnumerable<GMapMarker>)converter.Convert(new object[] { vm.CurrentLocation, vm.Waypoints }, typeof(System.Collections.ObjectModel.ObservableCollection<GMapMarker>), null, System.Globalization.CultureInfo.DefaultThreadCurrentUICulture);
            foreach (var marker in newdata)
                MainMap.Markers.Add(marker);
        }
    }
}