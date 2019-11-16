using Core;
using RED.ViewModels;
using System.Windows.Controls;

namespace RED.Views {
	public partial class ControlCenterView
    {
		Camera camera = null;

        public ControlCenterView()
        {
            InitializeComponent();
            MainTabs.SelectionChanged += MainTabs_SelectionChanged;

			CameraTest.MouseDown += CameraTest_MouseDown;

			camera = new Camera(1, CameraTest);
		}

		private void CameraTest_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed) camera.Close();
			if (e.RightButton == System.Windows.Input.MouseButtonState.Pressed) camera.Open();
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
