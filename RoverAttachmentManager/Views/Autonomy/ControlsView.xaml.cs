using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RoverAttachmentManager.Views.Autonomy
{
    /// <summary>
    /// Interaction logic for ControlsView.xaml
    /// </summary>
    public partial class ControlsView : UserControl
    {
        public ControlsView()
        {
            InitializeComponent();
        }

        public async void Calibrate_Click(object sender, RoutedEventArgs e)
        {
            await PromptCalibrate();
        }

        private Task PromptCalibrate()
        {
            throw new NotImplementedException();
        }
    }
}
