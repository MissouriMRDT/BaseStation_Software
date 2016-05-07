using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using RED.ViewModels.ControlCenter;
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

namespace RED.Views.ControlCenter
{
    /// <summary>
    /// Interaction logic for PowerView.xaml
    /// </summary>
    public partial class PowerView : UserControl
    {
        public PowerView()
        {
            InitializeComponent();
        }

        private async void RebootButton_Click(object sender, RoutedEventArgs e)
        {
            MetroDialogSettings settings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Reboot",
                NegativeButtonText = "Cancel",
                AnimateShow = false,
                AnimateHide = false
            };
            var result = await ((MetroWindow)MetroWindow.GetWindow(this)).ShowMessageAsync(
                title: "Rover Reboot",
                message: "This will command the Battery Management System to reboot the rover. Communications will be intrrupted until RED reconnects to the rover.",
                style: MessageDialogStyle.AffirmativeAndNegative,
                settings: settings);

            if (result == MessageDialogResult.Affirmative)
            {
                ((PowerViewModel)DataContext).RebootRover();
            }
        }

        private async void ShutDownButton_Click(object sender, RoutedEventArgs e)
        {
            MetroDialogSettings settings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Shut Down",
                NegativeButtonText = "Cancel",
                AnimateShow = false,
                AnimateHide = false
            };
            var result = await ((MetroWindow)MetroWindow.GetWindow(this)).ShowMessageAsync(
                title: "Rover Shut Down",
                message: "This will command the Battery Management System to shut down the rover. Communications will be interrupted. THIS CANNOT BE REVERSED REMOTELY!",
                style: MessageDialogStyle.AffirmativeAndNegative,
                settings: settings);

            if (result == MessageDialogResult.Affirmative)
            {
                ((PowerViewModel)DataContext).EStopRover();
            }
        }

        private async void EnableButton_Click(object sender, RoutedEventArgs e)
        {
            byte busIndex = Byte.Parse((string)((Button)sender).Tag);
            MetroDialogSettings settings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Enable",
                NegativeButtonText = "Cancel",
                AnimateShow = false,
                AnimateHide = false
            };
            var result = await ((MetroWindow)MetroWindow.GetWindow(this)).ShowMessageAsync(
                title: "Power Bus Enable",
                message: "This will command the Powerboard to enable Bus #" + busIndex.ToString() + ".",
                style: MessageDialogStyle.AffirmativeAndNegative,
                settings: settings);

            if (result == MessageDialogResult.Affirmative)
            {
                ((PowerViewModel)DataContext).EnableBus(busIndex);
            }
        }

        private async void DisableButton_Click(object sender, RoutedEventArgs e)
        {
            byte busIndex = Byte.Parse((string)((Button)sender).Tag);
            MetroDialogSettings settings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Disable",
                NegativeButtonText = "Cancel",
                AnimateShow = false,
                AnimateHide = false
            };
            var result = await ((MetroWindow)MetroWindow.GetWindow(this)).ShowMessageAsync(
                title: "Power Bus Disable",
                message: "This will command the Powerboard to disable Bus #" + busIndex.ToString() + ". If this bus powers communications equipment, communications will be interrupted.",
                style: MessageDialogStyle.AffirmativeAndNegative,
                settings: settings);

            if (result == MessageDialogResult.Affirmative)
            {
                ((PowerViewModel)DataContext).DisableBus(busIndex);
            }
        }
    }
}
