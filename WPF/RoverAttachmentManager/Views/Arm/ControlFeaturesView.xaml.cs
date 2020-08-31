using RoverAttachmentManager.ViewModels.Arm;
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

namespace RoverAttachmentManager.Views.Arm
{
    /// <summary>
    /// Interaction logic for ControlFeaturesView.xaml
    /// </summary>
    public partial class ControlFeaturesView : UserControl
    {
        public ControlFeaturesView()
        {
            InitializeComponent();
        }
        private void OverrideButton_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)((ToggleButton)sender).IsChecked)
            {
                ((ControlFeaturesViewModel)DataContext).LimitSwitchOverride();
            }
            else
            {
                ((ControlFeaturesViewModel)DataContext).LimitSwitchUnOverride();
            }
        }
    }
}
