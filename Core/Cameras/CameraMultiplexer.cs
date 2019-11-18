using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows.Controls;

namespace Core.Cameras {
	public static class CameraMultiplexer {
		private static readonly string BASE_ADDRESS = "131.151.19.185";
		private static readonly int NUMBER_OF_FEEDS = 3;

		private static bool initialized = false;
		static List<FeedInfo> feeds = new List<FeedInfo>();
		static Timer watchdog = new Timer();

		/// <summary>
		/// Initialize the multiplexer. Must be called before any feeds are accessed.
		/// </summary>
		public static void Initialize() {
			if(initialized) {
				CommonLog.Instance.Log("Warning: Camera multiplexer already initialized.");
				return;
			}

			for(int i = 1; i <= NUMBER_OF_FEEDS; i++) {
				Uri uri = new Uri($"http://{BASE_ADDRESS}:8080/{i}/stream");

				NewMjpegDecoder add = new NewMjpegDecoder();
				add.ParseStream(uri);
				add.FrameReady += Decoder_FrameReady;
				add.Error += Decoder_Error;

				feeds.Add(new FeedInfo() {
					URI = uri,
					Decoder = add,
					RenderSurfaces = new List<Image>(),
					LastFrameTime = DateTime.Now,
					LastFrame = new System.Drawing.Bitmap(640, 480)
				});
			}

			watchdog.Interval = 250;
			watchdog.Elapsed += Watchdog_Elapsed;
			watchdog.Start();

			initialized = true;
		}

		/// <summary>
		/// Maps a MjpegDecoder object to it's index. Should only be called by event handlers attached to a MjpegDecoder object.
		/// </summary>
		/// <param name="sender"></param>
		/// <returns></returns>
		private static int DecoderToIndex(NewMjpegDecoder sender) {
			int raw = -1;

			// TODO: Rewrite with LINQ?
			for(int i = 0; i < feeds.Count; i++) {
				if(feeds[i].Decoder == sender) { raw = i; break; }
			}

			if (raw == -1) throw new ArgumentOutOfRangeException("Passed MjpegDecoder object does not have an index");

			return raw;
		}

		/// <summary>
		/// Adds a rendering surface to display the output from a camera.
		/// </summary>
		/// <param name="index">Index of the camera feed to display.</param>
		/// <param name="surface">Name of an Image to display on.</param>
		public static void AddSurface(int index, Image surface) {
			if (index > feeds.Count) throw new ArgumentOutOfRangeException("Passed camera index is not valid");
			feeds[index].RenderSurfaces.Add(surface);
		}

		/// <summary>
		/// Gets the last displayed frame from a camera.
		/// </summary>
		/// <param name="index">Index of the camera feed to screenshot</param>
		/// <returns>Bitmap image of the feed</returns>
		public static System.Drawing.Bitmap GetScreenshot(int index) {
			if (index > feeds.Count) throw new ArgumentOutOfRangeException("Passed camera index is not valid");
			return feeds[index].LastFrame;
		}

		private static void Watchdog_Elapsed(object sender, ElapsedEventArgs e) {
			DateTime now = DateTime.Now;

			for(int i = 0; i < feeds.Count; i++) {
				FeedInfo fi = feeds[i];
				
				double downTime = new TimeSpan(now.Ticks - fi.LastFrameTime.Ticks).TotalMilliseconds;

				if (downTime >= 2500) {
					CommonLog.Instance.Log("Camera feed {0} has been unavailable for {1} milliseconds, reopening feed.", i + 1, downTime);
					fi.Decoder.StopStream();
					fi.Decoder.ParseStream(fi.URI);
				}

				else if (downTime >= 1000) {
					CommonLog.Instance.Log("Camera feed {0} unavailable.", i + 1);
				}
			}
		}

		private static void Decoder_Error(object sender, ErrorEventArgs e) {
			int index = DecoderToIndex((NewMjpegDecoder)sender);
			CommonLog.Instance.Log("Camera feed {0} error: {1}", index, e.Message);
		}

		private static void Decoder_FrameReady(object sender, FrameReadyEventArgs e) {
			int index = DecoderToIndex((NewMjpegDecoder)sender);
			foreach(Image surface in feeds[index].RenderSurfaces) { surface.Source = e.BitmapImage; }

			feeds[index].LastFrame = e.Bitmap;
			feeds[index].LastFrameTime = DateTime.Now;
		}
	}
}
