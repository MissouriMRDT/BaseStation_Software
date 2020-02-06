using RoverAttachmentManager.ViewModels.Science;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RoverAttachmentManager.Views.Science
{
    /// <summary>
    /// Interaction logic for SpectrometerView.xaml
    /// </summary>
    public partial class SpectrometerView : UserControl
    {
        public SpectrometerView()
        {
            InitializeComponent();
        }

        private void EnableButton_Click(object sender, RoutedEventArgs e)
        {
            ((SpectrometerViewModel)DataContext).SetUVLed((bool)((ToggleButton)sender).IsChecked ? (byte)1 : (byte)0);
        }
    }
}
