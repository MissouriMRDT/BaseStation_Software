using System.Windows;
using System.Windows.Controls;

namespace RED.Views
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

        private void HandleSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
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

        public class BoolToStringConverter : RED.Addons.BoolToValueConverter<string> { }
    }
}
