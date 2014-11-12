namespace RED.Views
{
    using System.Windows;
    using MahApps.Metro.Controls;

    public partial class ControlCenterView
    {
        public ControlCenterView()
        {
            InitializeComponent();
        }
        private void ToggleSettingsFlyout(object sender, RoutedEventArgs e)
        {
            Shell.ToggleFlyout(0);
        }
        private void ToggleLayoutsFlyout(object sender, RoutedEventArgs e)
        {
            Shell.ToggleFlyout(1);
        }
        private void ToggleFlyout(int index)
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
