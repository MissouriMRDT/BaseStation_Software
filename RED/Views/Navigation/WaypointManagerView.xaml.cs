using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using RED.ViewModels.Navigation;
using System.Windows;
using System.Windows.Controls;

namespace RED.Views.Navigation
{
    /// <summary>
    /// Interaction logic for WaypointManagerView.xaml
    /// </summary>
    public partial class WaypointManagerView : UserControl
    {
        public WaypointManagerView()
        {
            InitializeComponent();
        }

        private void AddWaypointBtn_Click(object sender, RoutedEventArgs e)
        {
            var vm = (WaypointManagerViewModel)DataContext;
            if (!vm.AddWaypoint("Untitled Waypoint", LatitudeTextBox.Text, LongitudeTextBox.Text))
            {
                MetroDialogSettings settings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "OK",
                    NegativeButtonText = "Cancel",
                    AnimateShow = false,
                    AnimateHide = false,
                };
                ((MetroWindow)MetroWindow.GetWindow(this)).ShowMessageAsync(
                    title: "Waypoint Management",
                    message: "Invalid Longitude or Latitude. Must be a floating point number.",
                    style: MessageDialogStyle.Affirmative,
                    settings: settings);
            }
        }
    }
}
