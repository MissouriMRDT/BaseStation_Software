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

        public class BoolToStringConverter : RED.Addons.BoolToValueConverter<string> { }
    }
}
