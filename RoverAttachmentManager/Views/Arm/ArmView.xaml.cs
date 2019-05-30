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
            if ((bool)((ToggleButton)sender).IsChecked)
            {
                ((ArmViewModel)DataContext).LimitSwitchOverride();
            }
            else
            {
                ((ArmViewModel)DataContext).LimitSwitchUnOverride();
            }
        }
    }
}
