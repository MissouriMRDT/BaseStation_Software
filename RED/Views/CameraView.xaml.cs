using Core.Cameras;
using RED.ViewModels;
using System;
using System.Windows.Controls;

namespace RED.Views
{
	/// <summary>
	/// Interaction logic for CameraView.xaml
	/// </summary>
	public partial class CameraView : UserControl
	{
		public CameraView()
		{
			InitializeComponent();

			DataContextChanged += CameraView_DataContextChanged;

			// Randomize the internal name of the image control so that calls to the multiplexer only affect this cameraview instance
			CameraFeed.Name = "Feed_" + Guid.NewGuid().ToString("N");
		}

		private void CameraView_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
		{
			if (DataContext.GetType() != typeof(CameraViewModel)) return;

			CameraViewModel cameraViewModel = ((CameraViewModel)(DataContext));
			cameraViewModel.CameraFeed = CameraFeed;
			cameraViewModel.SetCamera(Camera.LeftGimbal);
		}
	}
}
