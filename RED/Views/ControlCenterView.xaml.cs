using RED.ViewModels;

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
    }
}
