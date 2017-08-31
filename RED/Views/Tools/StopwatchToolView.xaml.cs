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

        private void FixTime_Click(object sender, RoutedEventArgs e)
        {
            FixContextMenu.IsEnabled = true;
            FixContextMenu.PlacementTarget = (Button)sender;
            FixContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            FixContextMenu.IsOpen = true;
        }

        private void SetTime_Click(object sender, RoutedEventArgs e)
        {
            FixContextMenu.IsOpen = false;
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
