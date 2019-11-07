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
			InitSelector();

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
            vm.AddWaypoint(vm.NewPoint.Name, vm.NewPoint.Latitude, vm.NewPoint.Longitude);
            
        }

		private void DMSSelector_Selected(object sender, RoutedEventArgs e)
		{
			
			var selector = (ComboBox)sender;
			var selected = selector.SelectedItem;
			if (selected.Equals(LonLat))
			{
				colDeg.Visibility = Visibility.Collapsed;
				colMin.Visibility = Visibility.Collapsed;
				colSec.Visibility = Visibility.Collapsed;
				colLon.Visibility = Visibility.Visible;
				colLat.Visibility = Visibility.Visible;
			}
			else
			{
				colDeg.Visibility = Visibility.Visible;
				colMin.Visibility = Visibility.Visible;
				colSec.Visibility = Visibility.Visible;
				colLon.Visibility = Visibility.Collapsed;
				colLat.Visibility = Visibility.Collapsed;
			}

		}

		private void AddWaypointGrid_Initialized(object sender, System.EventArgs e)
		{
			Grid grid = (Grid)sender;
			
		}

	private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{

	}

	private void InitSelector()
	{
	  DMSSelector.SelectedIndex = 0;
	  colDeg.Visibility = Visibility.Collapsed;
	  colMin.Visibility = Visibility.Collapsed;
	  colSec.Visibility = Visibility.Collapsed;
	  colLon.Visibility = Visibility.Visible;
	  colLat.Visibility = Visibility.Visible;
	}
  }
}
