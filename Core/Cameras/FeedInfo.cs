using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Core.Cameras {
	internal class FeedInfo {
		/// <summary>
		/// MjpegDecoder object for this stream
		/// </summary>
		public MjpegDecoder Decoder { get; set; }

		/// <summary>
		/// URI for the camera feed
		/// </summary>
		public Uri URI { get; set; }

		/// <summary>
		/// Motion camera stream index
		/// </summary>
		public int Index { get; internal set; }

		/// <summary>
		/// Destination rendering surfaces to send decoded images to
		/// </summary>
		public List<Image> RenderSurfaces { get; set; }

		/// <summary>
		/// Timestamp of the last received frame
		/// </summary>
		public DateTime LastFrameTime { get; set; }

		/// <summary>
		/// Last received frame from this feed
		/// </summary>
		public System.Windows.Media.Imaging.BitmapImage LastFrame { get; set; }
	}
}
