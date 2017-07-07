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

        private void HandleSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
        }

        private void AddWaypointBtn_Click(object sender, RoutedEventArgs e)
        {
            var vm = (WaypointManagerViewModel)DataContext;
            if (!vm.AddWaypoint("Untitled Waypoint", LatitudeTextBox.Text, LongitudeTextBox.Text))
                MessageBox.Show("Invalid Longitude or Latitude. Must be a floating point number.");
        }
    }
}
