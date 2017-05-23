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

        private void OnOffButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = ((InputSelectorViewModel)DataContext);
            if (vm.IsRunning)
                vm.Stop();
            else
                vm.Start();
        }
    }

    public class BoolToStringConverter : RED.Addons.BoolToValueConverter<string> { }
}
