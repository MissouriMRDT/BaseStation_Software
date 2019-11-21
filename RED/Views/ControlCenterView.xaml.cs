using Core.Cameras;
using RED.ViewModels;
using System;
using System.Drawing;
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
			if (e.RightButton == System.Windows.Input.MouseButtonState.Pressed) {
				DateTime now = DateTime.Now;
				string fn = $"camera{index}-{now.Year}-{now.Month}-{now.Day}-{now.Hour}-{now.Minute}-{now.Second}.jpg";

				string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Rover Screenshots");
				System.IO.Directory.CreateDirectory(path);
				path = System.IO.Path.Combine(path, fn);

				Bitmap i = CameraMultiplexer.Screenshot(index);
				i.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);

				Core.CommonLog.Instance.Log("Screenshot of camera stream {0} saved to {1}", index, path);
			}

			else if(e.LeftButton == System.Windows.Input.MouseButtonState.Pressed) {
				CameraMultiplexer.RemoveAllSurfaces(index);
				if(++index > CameraMultiplexer.TotalCameraFeeds) { index = 1; }

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
