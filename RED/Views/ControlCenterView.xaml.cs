using RED.ViewModels;
using System.Windows.Controls;

namespace RED.Views
{
    public partial class ControlCenterView
    {
        public ControlCenterView()
        {
            InitializeComponent();
            MainTabs.SelectionChanged += MainTabs_SelectionChanged;
        }

        private void MainTabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.OriginalSource == MainTabs && e.RemovedItems.Count > 0)
            {
                var tab = e.RemovedItems[0] as TabItem;
                if (tab != null && tab == SettingsTab)
                {
                    var vm = (ControlCenterViewModel)DataContext;
                    vm.SettingsManager.SaveSettings();
                }
            }
        }
    }
}
