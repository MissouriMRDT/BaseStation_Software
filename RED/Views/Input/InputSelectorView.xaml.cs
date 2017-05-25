using RED.ViewModels.Input;
using System.Windows;
using System.Windows.Controls;

namespace RED.Views.Input
{
    /// <summary>
    /// Interaction logic for InputSelectorView.xaml
    /// </summary>
    public partial class InputSelectorView : UserControl
    {
        public InputSelectorView()
        {
            InitializeComponent();
        }

        private void HandleSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
        }
    }

    public class BoolToStringConverter : RED.Addons.BoolToValueConverter<string> { }
}
