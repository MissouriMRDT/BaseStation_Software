using Core.Cameras;
using RED.ViewModels;
using System.Windows.Controls;

namespace RED.Views {
	public partial class ControlCenterView {
		int index = 1;

        public ControlCenterView()
        {
            InitializeComponent();
            MainTabs.SelectionChanged += MainTabs_SelectionChanged;
			CameraTest.MouseDown += CameraTest_MouseDown;

			CameraMultiplexer.Initialize();
			CameraMultiplexer.AddSurface(1, CameraTest);
			CameraMultiplexer.AddSurface(2, SecondCamera);
			CameraMultiplexer.AddSurface(3, ThirdCamera);
		}

		private void CameraTest_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            Image camera = (Image)sender;
            index = (int)camera.Tag;

			if (e.RightButton == System.Windows.Input.MouseButtonState.Pressed) {
				CameraMultiplexer.Screenshot(index);
			}

			else if(e.LeftButton == System.Windows.Input.MouseButtonState.Pressed) {
				CameraMultiplexer.RemoveAllSurfaces(index);
				CameraMultiplexer.AddSurface(index, CameraTest);
			}
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
