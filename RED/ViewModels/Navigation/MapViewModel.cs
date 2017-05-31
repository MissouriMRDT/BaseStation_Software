using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using Caliburn.Micro;
using RED.Addons;
using RED.Models.Navigation;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Shapes;

namespace RED.ViewModels.Navigation
{
    public class MapViewModel : PropertyChangedBase
    {
        MapModel _model;

        public Waypoint CurrentLocation
        {
            get
            {
                return _model.currentLocation;
            }
            set
            {
                _model.currentLocation = value;
                NotifyOfPropertyChange(() => CurrentLocation);
            }
        }
        public ObservableCollection<Waypoint> Waypoints
        {
            get
            {
                return _model.waypoints;
            }
            set
            {
                _model.waypoints = value;
                NotifyOfPropertyChange(() => Waypoints);
            }
        }

        private GMapControl mainMap = new GMapControl();
        public GMapControl MainMap
        {
            get
            {
                return mainMap;
            }
        }

        public MapViewModel()
        {
            _model = new MapModel();
            InitializeMapControl();

            Waypoints.Add(new Waypoint(37.951631, -91.777713)); //Rolla
            Waypoints.Add(new Waypoint(37.850025, -91.701845)); //Fugitive Beach
            Waypoints.Add(new Waypoint(38.406426, -110.791919)); //Mars Desert Research Station
            RefreshMap();
        }

        private void InitializeMapControl()
        {
            MainMap.Margin = new Thickness(-5);

            MainMap.Manager.Mode = AccessMode.CacheOnly;
            MainMap.MapProvider = GMapProviders.OpenStreetMapQuestHybrid;
            MainMap.Position = new PointLatLng(RED.Properties.Settings.Default.GPSStartLocationLatitude, RED.Properties.Settings.Default.GPSStartLocationLongitude);
            MainMap.MinZoom = 1;
            MainMap.MaxZoom = 18;
            MainMap.Zoom = 10;

            MainMap.EmptyMapBackground = Brushes.White;
            MainMap.EmptytileBrush = Brushes.White;
            MainMap.EmptyTileText = new FormattedText("No imagery available in cache.", System.Globalization.CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, new Typeface("Sans Serif"), SystemFonts.MessageFontSize, Brushes.Black);

            MainMap.IgnoreMarkerOnMouseWheel = true;
            MainMap.MouseWheelZoomType = MouseWheelZoomType.MousePositionWithoutCenter;
            //MainMap.FillEmptyTiles = false; //Good for debugging map cache
        }

        public void CacheImport()
        {
            MainMap.ShowImportDialog();
        }
        public void CacheExport()
        {
            MainMap.ShowExportDialog();
        }
        public void CachePrefetch()
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
        public void CacheClear()
        {
            if (MessageBox.Show("Are You sure?", "Clear GMap.NET cache?", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                try
                {
                    MainMap.Manager.PrimaryCache.DeleteOlderThan(System.DateTime.Now, null);
                    MessageBox.Show("Done. Cache is clear.");
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void RefreshMap()
        {
            MainMap.Markers.Clear();

            var converter = new RED.Addons.GMapMarkerCollectionMultiConverter();
            var newdata = (IEnumerable<GMapMarker>)converter.Convert(new object[] { CurrentLocation, Waypoints }, typeof(System.Collections.ObjectModel.ObservableCollection<GMapMarker>), null, System.Globalization.CultureInfo.DefaultThreadCurrentUICulture);
            foreach (var marker in newdata)
                MainMap.Markers.Add(marker);
        }
    }
}
