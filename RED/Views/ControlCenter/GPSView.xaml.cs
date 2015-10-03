using GMap.NET;
using GMap.NET.WindowsPresentation;
using GMap.NET.MapProviders;
using RED.ViewModels.ControlCenter;
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
        public GPSView()
        {
            InitializeComponent();

            MainMap.Manager.Mode = AccessMode.CacheOnly;
            MainMap.MapProvider = GMapProviders.OpenStreetMapQuestHybrid;
            MainMap.Position = new PointLatLng(50.782542, 20.462027);//Hanksville=(38.373933, -110.708362);//Rolla=(37.848544, -91.7715303)
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
                double xCoord, yCoord;
                string[] latInputs = LatitudeTextBox.Text.Trim().Split((string[])null, StringSplitOptions.RemoveEmptyEntries);
                switch (latInputs.Length)
                {
                    case 0: throw new ArgumentException();
                    case 1: if (!Double.TryParse(latInputs[0], out yCoord)) throw new ArgumentException(); break;
                    case 2:
                        {
                            int value0;
                            double value1;
                            if (!Int32.TryParse(latInputs[0], out value0) || !Double.TryParse(latInputs[1], out value1)) throw new ArgumentException();
                            yCoord = (value0) + Math.Sign(value0) * (value1 * 1 / 60d);
                        } break;
                    case 3:
                        {
                            int value0, value1;
                            double value2;
                            if (!Int32.TryParse(latInputs[0], out value0) || !Int32.TryParse(latInputs[1], out value1) || !Double.TryParse(latInputs[2], out value2)) throw new ArgumentException();
                            yCoord = (value0) + Math.Sign(value0) * ((value1 * 1 / 60d) + (value2 * 1 / 60d / 60d));
                        } break;
                    default: throw new ArgumentException();
                }
                string[] lonInputs = LongitudeTextBox.Text.Trim().Split((string[])null, StringSplitOptions.RemoveEmptyEntries);
                switch (lonInputs.Length)
                {
                    case 0: throw new ArgumentException();
                    case 1: if (!Double.TryParse(lonInputs[0], out xCoord)) throw new ArgumentException(); break;
                    case 2:
                        {
                            int value0;
                            double value1;
                            if (!Int32.TryParse(lonInputs[0], out value0) || !Double.TryParse(lonInputs[1], out value1)) throw new ArgumentException();
                            xCoord = (value0) + Math.Sign(value0) * (value1 * 1 / 60d);
                        } break;
                    case 3:
                        {
                            int value0, value1;
                            double value2;
                            if (!Int32.TryParse(lonInputs[0], out value0) || !Int32.TryParse(lonInputs[1], out value1) || !Double.TryParse(lonInputs[2], out value2)) throw new ArgumentException();
                            xCoord = (value0) + Math.Sign(value0) * ((value1 * 1 / 60d) + (value2 * 1 / 60d / 60d));
                        } break;
                    default: throw new ArgumentException();
                }
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