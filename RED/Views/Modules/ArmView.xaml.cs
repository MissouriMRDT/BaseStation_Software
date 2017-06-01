using System.Windows.Controls;

namespace RED.Views.Modules
{
    /// <summary>
    /// Interaction logic for ArmView.xaml
    /// </summary>
    public partial class ArmView : UserControl
    {
        public ArmView()
        {
            InitializeComponent();
        }

        private void HandleSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
        }
    }
}
