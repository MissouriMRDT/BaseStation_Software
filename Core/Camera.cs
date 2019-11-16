using MjpegProcessor;
using System;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Core {
	public class Camera {
		private MjpegDecoder mjpeg = new MjpegDecoder();
		private Image output;
		int id;
		bool open = false;

		Timer watchdog = new Timer();
		DateTime lastFrame = DateTime.MinValue;

		public Camera(int index, Image image) {
			output = image;
			id = index;
			watchdog.Interval = 250;

			Open();

			mjpeg.FrameReady += Mjpeg_FrameReady;
			mjpeg.Error += Mjpeg_Error;
			watchdog.Tick += Watchdog_Tick;
		}

		public void Open() {
			if (open) CommonLog.Instance.Log("Refusing to open camera stream multiple times.");

			watchdog.Start();
			mjpeg.ParseStream(new Uri($"http://131.151.19.185:8080/{id}/stream"));
			open = true;
		}

		public void Close() {
			if (!open) CommonLog.Instance.Log("Cannot close an already closed camera stream.");

			watchdog.Stop();
			mjpeg.StopStream();
			open = false;
		}

		public void CloseAndReopen(int newIndex = -1) {
			Close();

			if (newIndex != -1) {
				id = newIndex;
				open = false;
			}

			Open();
		}

		private void Mjpeg_FrameReady(object sender, FrameReadyEventArgs e) {
			output.Source = e.BitmapImage;
			lastFrame = DateTime.Now;
		}

		private void Watchdog_Tick(object sender, EventArgs e) {
			double downTime = new TimeSpan(DateTime.Now.Ticks - lastFrame.Ticks).TotalMilliseconds;

			if (downTime >= 2000) {
				CommonLog.Instance.Log("Stream was interrupted, restarting feed (last frame was {0} miliseconds ago).", downTime);
				CloseAndReopen();
			}

			else if(downTime >= 800) {
				CommonLog.Instance.Log("Last received frame was {0} miliseconds ago!", downTime);
			}
		}

		private void Mjpeg_Error(object sender, ErrorEventArgs e) {
			CommonLog.Instance.Log("Error code {0} opening camera stream: {1}", e.ErrorCode, e.Message);
			open = false;
		}
	}
}
