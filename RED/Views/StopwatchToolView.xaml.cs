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

        public class BoolToStringConverter : RED.Addons.BoolToValueConverter<string> { }
    }
}
