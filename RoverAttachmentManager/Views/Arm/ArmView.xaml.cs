using RoverAttachmentManager.ViewModels.Arm;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace RoverAttachmentManager.Views.Arm
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

        private async void OverrideButton_Click(object sender, RoutedEventArgs e)
        {
            byte busIndex1 = Byte.Parse((string)((ToggleButton)sender).Tag);
            byte busIndex2 = Byte.Parse((string)((ToggleButton)sender).Tag + 1);
            if ((bool)((ToggleButton)sender).IsChecked)
            {
                ((ArmViewModel)DataContext).LimitSwitchOverride(busIndex1);
                ((ArmViewModel)DataContext).LimitSwitchOverride(busIndex2);
            }
            else
            {
                ((ArmViewModel)DataContext).LimitSwitchUnOverride(busIndex1);
                ((ArmViewModel)DataContext).LimitSwitchUnOverride(busIndex2);
            }
        }
    }
}
