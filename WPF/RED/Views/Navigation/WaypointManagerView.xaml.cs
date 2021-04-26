using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using RED.ViewModels.Navigation;
using System.Windows;
using System.Windows.Controls;

namespace RED.Views.Navigation
{
    /// <summary>
    /// Interaction logic for WaypointManagerView.xaml
    /// </summary>
    public partial class WaypointManagerView : UserControl
    {
	    public WaypointManagerView()
        {
            InitializeComponent();
		}

        private void AddMenu_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            btn.ContextMenu.IsEnabled = true;
            btn.ContextMenu.PlacementTarget = btn;
            btn.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            btn.ContextMenu.IsOpen = true;
        }

        private void AddWaypointBtn_Click(object sender, RoutedEventArgs e)
        {
            var vm = (WaypointManagerViewModel)DataContext;
            vm.AddWaypoint(vm.Name, vm.Latitude, vm.Longitude);
        }

        private void AddWaypointDMSBtn_Click(object sender, RoutedEventArgs e)
        {
            var vm = (WaypointManagerViewModel)DataContext;
            vm.AddWaypoint(vm.Name, vm.LatitudeD, vm.LatitudeM, vm.LatitudeS, vm.LongitudeD, vm.LongitudeM, vm.LongitudeS);
        }
    }
}
