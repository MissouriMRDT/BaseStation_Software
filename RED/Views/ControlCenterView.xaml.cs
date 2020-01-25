using Core.Cameras;
using RED.ViewModels;
using System.Windows.Controls;
using System.Windows.Media;

namespace RED.Views {
	public partial class ControlCenterView {
		public ControlCenterView()
		{
			InitializeComponent();
			MainTabs.SelectionChanged += MainTabs_SelectionChanged;
			CameraTest.MouseDown += CameraTest_MouseDown;
			SecondCamera.MouseDown += CameraTest_MouseDown;
			ThirdCamera.MouseDown += CameraTest_MouseDown;
			FourthCamera.MouseDown += CameraTest_MouseDown;

			CameraMultiplexer.Initialize();
			CameraMultiplexer.AddSurface(1, CameraTest);
			CameraMultiplexer.AddSurface(2, SecondCamera);
			CameraMultiplexer.AddSurface(3, ThirdCamera);
			CameraMultiplexer.AddSurface(4, FourthCamera);
		}

		private void CameraTest_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			Image camera = (Image)sender;
			int current = int.Parse(camera.Tag.ToString());

			if(e.LeftButton == System.Windows.Input.MouseButtonState.Pressed) {
				CameraMultiplexer.RemoveSurface(camera);

				if (++current > CameraMultiplexer.TotalCameraFeeds) current = 1;
				CameraMultiplexer.AddSurface(current, camera);

				camera.Tag = current;
			}

			else if(e.RightButton == System.Windows.Input.MouseButtonState.Pressed) {
				camera.RenderTransform = new RotateTransform(90, camera.ActualWidth / 2, camera.ActualHeight / 2);
			}

			else if(e.MiddleButton == System.Windows.Input.MouseButtonState.Pressed) {
				CameraMultiplexer.Screenshot(current);
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
