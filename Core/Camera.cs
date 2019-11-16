using MjpegProcessor;
using System;
using System.Windows.Media.Imaging;

namespace Core {
	public class Camera {
		private MjpegDecoder mjpeg = new MjpegDecoder();
		private EventHandler<BitmapImage> handler;

		public Camera(int id, EventHandler<BitmapImage> argument) {
			handler = argument;

			mjpeg.ParseStream(new Uri($"http://131.151.19.185:8080/{id}/stream"));
			mjpeg.FrameReady += Mjpeg_FrameReady;
			mjpeg.Error += Mjpeg_Error;
		}

		private void Mjpeg_Error(object sender, ErrorEventArgs e) {
			CommonLog.Instance.Log("Error code {1} opening camera stream: {2}", e.ErrorCode, e.Message);
		}

		private void Mjpeg_FrameReady(object sender, FrameReadyEventArgs e) {
			handler.Invoke(null, e.BitmapImage);
		}
	}
}
