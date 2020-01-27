using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Media.Imaging;
using System.IO;

namespace Core.Cameras {
	public static class CameraMultiplexer {
		private static readonly List<string> BASE_ADDRESSES = new List<string>()
		{
			"192.168.1.50",
			"192.168.1.51"
		};

		private static bool initialized = false;
		static List<FeedInfo> feeds = new List<FeedInfo>();
		static Timer watchdog = new Timer();
		static int watchdogSkip = 0;

		/// <summary>
		/// Returns the total number of connected cameras.
		/// </summary>
		public static int TotalCameraFeeds { get; private set; } = 4;

		/// <summary>
		/// Initialize the multiplexer. Must be called before any feeds are accessed.
		/// </summary>
		public static void Initialize() {
			if(initialized) {
				CommonLog.Instance.Log("Warning: Camera multiplexer already initialized.");
				return;
			}
			
			for(int i = 1; i <= TotalCameraFeeds; i++) {
				Uri uri = ConstructAddress(i);

				MjpegDecoder add = new MjpegDecoder();
				add.ParseStream(uri);
				add.FrameReady += Decoder_FrameReady;
				add.Error += Decoder_Error;

				feeds.Add(new FeedInfo() {
					URI = uri,
					Index = i,
					Decoder = add,
					RenderSurfaces = new List<Image>(),
					LastFrameTime = DateTime.Now,
					LastFrame = new BitmapImage()
				});
			}

			watchdog.Interval = 1000;
			watchdog.Elapsed += Watchdog_Elapsed;
			watchdog.Start();

			initialized = true;
		}

		private static Uri ConstructAddress(int camera)
		{
			int index = (camera <= 4) ? 0 : 1;
			string addr = BASE_ADDRESSES[index];

			return new Uri($"http://{addr}:8080/{camera}/stream");
		}

		/// <summary>
		/// Maps a MjpegDecoder object to it's index. Should only be called by event handlers attached to a MjpegDecoder object.
		/// </summary>
		/// <param name="sender"></param>
		/// <returns></returns>
		private static int DecoderToIndex(MjpegDecoder sender) {
			try {
				using (IEnumerator<FeedInfo> e = feeds.Where(x => x.Decoder.UUID == sender.UUID).GetEnumerator()) {
					e.MoveNext();
					return e.Current.Index;
				}
			} catch(NullReferenceException ex) {
				throw new ArgumentOutOfRangeException("Passed MjpegDecoder object does not have an index. This is an internal error in the multiplexer class and needs to be reported to the signals team.", ex);
			}
		}

		/// <summary>
		/// Adds a rendering surface to display the output from a camera. This function is provided for backwards compatibility and will be removed in a future release.
		/// </summary>
		[Obsolete]
		public static void AddSurface(int index, Image surface)
		{
			if (index > feeds.Count) throw new ArgumentOutOfRangeException("Passed camera index is not valid");
			feeds[index - 1].RenderSurfaces.Add(surface);
		}

		/// <summary>
		/// Adds a rendering surface to display the output from a camera.
		/// </summary>
		/// <param name="camera">Camera feed to display.</param>
		/// <param name="surface">Name of an Image to display on.</param>
		public static void AddSurface(Camera camera, Image surface) {
			AddSurface((int)camera, surface);
		}

		public static void RemoveSurface(Image surface) {
			for(int i = 0; i <= TotalCameraFeeds - 1; i++) {
				Image match = feeds[i].RenderSurfaces.Find(new Predicate<Image>(x => x.Name == surface.Name));
				if(match != null) feeds[i].RenderSurfaces.Remove(match);
			}
		}

		/// <summary>
		/// Saves and returns the last displayed frame from a camera.
		/// </summary>
		/// <param name="camera">Camera stream to screenshot</param>
		/// <returns>Bitmap image of the feed</returns>
		public static System.Drawing.Bitmap Screenshot(Camera camera)
		{
			return Screenshot((int)camera);
		}

		/// <summary>
		/// Saves and returns the last displayed frame from a camera. This function is provided for backwards compatibility and will be removed in a future release.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		[Obsolete]
		public static System.Drawing.Bitmap Screenshot(int index) {
			if (index > feeds.Count) throw new ArgumentOutOfRangeException("Passed camera index is not valid");
			System.Drawing.Bitmap img = ConvertBitmapImageToBitmap(feeds[index - 1].LastFrame);

			DateTime now = DateTime.Now;
			string fn = $"camera{Pad(index)}-{Pad(now.Year)}-{Pad(now.Month)}-{Pad(now.Day)}_{Pad(now.Hour)}-{Pad(now.Minute)}-{Pad(now.Second)}.jpg";

			string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Rover Screenshots");
			Directory.CreateDirectory(path);
			path = Path.Combine(path, fn);
			
			img.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);

			CommonLog.Instance.Log("Screenshot of camera stream {0} saved to {1}", index, path);

			return img;
		}

		public static string Pad(int raw)
		{
			return raw.ToString().PadLeft(2, '0');
		}

		/// <summary>
		/// Converts a BitmapImage to a Bitmap
		/// </summary>
		/// <param name="bitmapImage">Input BitmapImage to convert</param>
		/// <returns>Converted Bitmap</returns>
		private static System.Drawing.Bitmap ConvertBitmapImageToBitmap(BitmapImage bitmapImage) {
			using (MemoryStream outStream = new MemoryStream()) {
				BitmapEncoder enc = new BmpBitmapEncoder();
				enc.Frames.Add(BitmapFrame.Create(bitmapImage));
				enc.Save(outStream);
				System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

				return new System.Drawing.Bitmap(bitmap);
			}
		}

		private static void Watchdog_Elapsed(object sender, ElapsedEventArgs e) {
			// ensures the watchdog doesn't stop a pending reconnection
			if (watchdogSkip > 0) { watchdogSkip--; return; }

			DateTime now = DateTime.Now;

			for(int i = 0; i < feeds.Count; i++) {
				FeedInfo fi = feeds[i];
				
				double downTime = new TimeSpan(now.Ticks - fi.LastFrameTime.Ticks).TotalMilliseconds;

				if (downTime >= 2000) {
					CommonLog.Instance.Log("Camera feed {0} has been unavailable for {1} milliseconds, reopening feed.", i + 1, downTime);
					fi.Decoder.StopStream();
					fi.Decoder.ParseStream(fi.URI);
					watchdogSkip = 3;
				}
			}
		}

		private static void Decoder_Error(object sender, ErrorEventArgs e) {
			int index = DecoderToIndex((MjpegDecoder)sender);
			CommonLog.Instance.Log("Camera feed {0} error: {1}", index, e.Message);
		}

		private static void Decoder_FrameReady(object sender, FrameReadyEventArgs e) {
			int index = DecoderToIndex((MjpegDecoder)sender);
			foreach(Image surface in feeds[index - 1].RenderSurfaces) { surface.Source = e.BitmapImage; }

			feeds[index - 1].LastFrame = e.BitmapImage;
			feeds[index - 1].LastFrameTime = DateTime.Now;
		}
	}

	public enum Camera
	{
		LeftGimbal = 1,
		RightGimbal,
		LeftSuspension,
		RightSuspension,
		LeftEndEffector,
		RightEndEffector,
		Elbow,
		Actuation,
		SensorBox,
		Carousel
	}
}
