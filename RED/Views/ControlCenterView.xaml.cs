using MahApps.Metro.Controls;
using RED.ViewModels;
using System.Windows;

namespace RED.Views
{
    public partial class ControlCenterView
    {
        public ControlCenterView()
        {
            InitializeComponent();
            MainTabs.SelectionChanged += MainTabs_SelectionChanged;
        }

        private void MainTabs_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count > 0 && (System.Windows.Controls.TabItem)(e.RemovedItems[0]) == SettingsTab)
            {
                var vm = DataContext as ControlCenterViewModel;
                vm.SettingsManager.SaveSettings();
            }
        }

        private void ToggleSettingsFlyout(object sender, RoutedEventArgs e)
        {
            Shell.ToggleFlyout(0);
        }
        private void ToggleLayoutsFlyout(object sender, RoutedEventArgs e)
        {
            Shell.ToggleFlyout(1);
        }
        internal void ToggleFlyout(int index)
        {
            var flyout = Flyouts.Items[index] as Flyout;
            if (flyout == null)
            {
                return;
            }

            flyout.IsOpen = !flyout.IsOpen;
        }
    }
}
