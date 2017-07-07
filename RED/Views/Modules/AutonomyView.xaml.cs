using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using RED.ViewModels.Modules;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RED.Views.Modules
{
    /// <summary>
    /// Interaction logic for AutonomyView.xaml
    /// </summary>
    public partial class AutonomyView : UserControl
    {
        public AutonomyView()
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
                ((AutonomyViewModel)DataContext).Calibrate();
            }
        }
    }
}
