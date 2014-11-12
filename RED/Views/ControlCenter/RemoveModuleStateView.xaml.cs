namespace RED.Views.ControlCenter
{
    using MahApps.Metro.Controls.Dialogs;
    using System.Windows;

    public partial class RemoveModuleStateView
    {
        private readonly ControlCenterView _window;

        public RemoveModuleStateView(ControlCenterView parent)
        {
            _window = parent;
            InitializeComponent();
        }
        private async void CloseDialog(object sender, RoutedEventArgs e)
        {
            await _window.HideMetroDialogAsync(this);
            _window.ToggleFlyout(1);
        }
    }
}
