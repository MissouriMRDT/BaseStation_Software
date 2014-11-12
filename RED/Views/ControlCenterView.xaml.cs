namespace RED.Views
{
    using System.Windows;
    using ControlCenter;
    using MahApps.Metro.Controls;
    using MahApps.Metro.Controls.Dialogs;

    public partial class ControlCenterView
    {
        private readonly SaveModuleStateView _saveDialog;
        private readonly RemoveModuleStateView _removeDialog;

        public ControlCenterView()
        {
            _removeDialog = new RemoveModuleStateView(this);
            _saveDialog = new SaveModuleStateView(this);
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
        internal void ToggleFlyout(int index)
        {
            var flyout = Flyouts.Items[index] as Flyout;
            if (flyout == null)
            {
                return;
            }

            flyout.IsOpen = !flyout.IsOpen;
        }

        private async void ShowSavePresetDialog(object sender, RoutedEventArgs e)
        {
            var dialog = (BaseMetroDialog)_saveDialog;
            ToggleFlyout(1);
            await this.ShowMetroDialogAsync(dialog);
        }
        private async void ShowDeletePresetDialog(object sender, RoutedEventArgs e)
        {
            var dialog = (BaseMetroDialog)_removeDialog;
            ToggleFlyout(1);
            await this.ShowMetroDialogAsync(dialog);
        }
    }
}
