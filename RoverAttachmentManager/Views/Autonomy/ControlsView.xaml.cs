using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using RoverAttachmentManager.ViewModels.Autonomy;
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

        private async Task PromptCalibrate()
        {
            MetroDialogSettings settings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Calibrate",
                NegativeButtonText = "Cancel",
                AnimateShow = false,
                AnimateHide = false
            };
            var result = await ((MetroWindow)MetroWindow.GetWindow(this)).ShowMessageAsync(
                title: "Calibrate Autonomy System",
                message: "This will command the autonomy board to begin the automatic calibration process. The rover should be pointed due north before starting.",
                style: MessageDialogStyle.AffirmativeAndNegative,
                settings: settings);

            if (result == MessageDialogResult.Affirmative)
            {
                ((ControlsViewModel)DataContext).Calibrate();
            }
        }
    }
}
