using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using RED.ViewModels.Navigation;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RED.Views.Navigation
{
    /// <summary>
    /// Interaction logic for MapView.xaml
    /// </summary>
    public partial class MapView : UserControl
    {
        public MapView()
        {
            InitializeComponent();
        }

        private void Map_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (DataContext.GetType() == typeof(MapViewModel))
                ((MapViewModel)(DataContext)).SetMap(Gmap);
        }

        private void CacheControl_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            btn.ContextMenu.IsEnabled = true;
            btn.ContextMenu.PlacementTarget = btn;
            btn.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            btn.ContextMenu.IsOpen = true;
        }

        private void CacheImportButton_Click(object sender, RoutedEventArgs e)
        {
            ((MapViewModel)DataContext).CacheImport();
        }
        private void CacheExportButton_Click(object sender, RoutedEventArgs e)
        {
            ((MapViewModel)DataContext).CacheExport();
        }
        private async void CachePrefetchButton_Click(object sender, RoutedEventArgs e)
        {
            var dc = (MapViewModel)DataContext;
            var result = await ShowMessage(
                title: "Map Cache Download",
                message: "Are you ready to download imagery between zoom=" + dc.CachePrefetchStartZoom + " and zoom=" + dc.CachePrefetchStopZoom + "?",
                buttonText: "Download");

            if (result == MessageDialogResult.Affirmative)
                dc.CachePrefetch();
        }
        private async void CacheClearButton_Click(object sender, RoutedEventArgs e)
        {
            var result = await ShowMessage(
             title: "Map Cache Clear",
             message: "Are you sure you want to clear the working cache?",
             buttonText: "Clear");

            if (result == MessageDialogResult.Affirmative)
                ((MapViewModel)DataContext).CacheClear();
        }

        private Task<MessageDialogResult> ShowMessage(string title, string message, string buttonText)
        {
            MetroDialogSettings settings = new MetroDialogSettings()
            {
                AffirmativeButtonText = buttonText,
                NegativeButtonText = "Cancel",
                AnimateShow = false,
                AnimateHide = false
            };
            return ((MetroWindow)MetroWindow.GetWindow(this)).ShowMessageAsync(
                title: title,
                message: message,
                style: MessageDialogStyle.AffirmativeAndNegative,
                settings: settings);
        }
    }
}
