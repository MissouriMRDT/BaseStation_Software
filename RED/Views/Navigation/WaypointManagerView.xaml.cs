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
            vm.AddWaypoint(vm.NewPoint.Name, vm.NewPoint.Latitude, vm.NewPoint.Longitude);
            
        }

		private void DMSSelector_Selected(object sender, RoutedEventArgs e)
		{
			
			var selector = (ComboBox)sender;
			var selected = selector.SelectedItem;
			System.Console.WriteLine("You Selected" + selected);
			if (selected.Equals(Degrees))
			{
				
			}
			else
			{
				
	        }

		}

		private void DMSSelector_Update(object comboBox, bool isDegrees)
		{
			//IsDegrees determines if the display is updated to show degrees or DMS
			if (isDegrees)
			{
                
			}
			else
			{
				
			}
		}

		private void AddWaypointGrid_Initialized(object sender, System.EventArgs e)
		{
			Grid grid = (Grid)sender;
			
		}

	private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{

	}
  }
}
