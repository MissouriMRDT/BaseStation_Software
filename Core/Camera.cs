using MjpegProcessor;
using System;
using System.Windows.Controls;

namespace Core {
	public class Camera {
		private MjpegDecoder mjpeg = new MjpegDecoder();
		private Image output;
		int id;
		bool open = false;

		public Camera(int index, Image image) {
			output = image;
			id = index;

			Open();

			mjpeg.FrameReady += Mjpeg_FrameReady;
			mjpeg.Error += Mjpeg_Error;
		}

		public void Open() {
			if (open) throw new InvalidOperationException("Camera is already open.");

			mjpeg.ParseStream(new Uri($"http://131.151.19.185:8080/{id}/stream"));
			open = true;
		}

		public void Close() {
			if (!open) throw new InvalidOperationException("Camera is already closed.");

			mjpeg.StopStream();
			open = false;
		}

		public void CloseAndReopen() {
			Close();
			Open();
		}

		private void Mjpeg_FrameReady(object sender, FrameReadyEventArgs e) {
			output.Source = e.BitmapImage;
		}

		private void Mjpeg_Error(object sender, ErrorEventArgs e) {
			CommonLog.Instance.Log("Error code {0} opening camera stream: {1}", e.ErrorCode, e.Message);
			open = false;
		}
	}
}
