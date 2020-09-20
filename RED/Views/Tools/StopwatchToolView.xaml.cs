using System.Windows;
using System.Windows.Controls;

namespace RED.Views.Tools
{
    /// <summary>
    /// Interaction logic for StopwatchToolView.xaml
    /// </summary>
    public partial class StopwatchToolView : UserControl
    {
        public StopwatchToolView()
        {
            InitializeComponent();
        }

        private void EditTime_Click(object sender, RoutedEventArgs e)
        {
            EditContextMenu.IsEnabled = true;
            EditContextMenu.PlacementTarget = (Button)sender;
            EditContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            EditContextMenu.IsOpen = true;
        }

		
	}
}
