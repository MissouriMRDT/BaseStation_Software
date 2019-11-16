using MjpegProcessor;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Core {
	public struct Output {
		public int id;
		public List<Image> surfaces;
		public MjpegDecoder decode;
	}

	public static class CameraMultiplexer {
		static MjpegDecoder first = new MjpegDecoder();
		static MjpegDecoder second = new MjpegDecoder();
		static MjpegDecoder third = new MjpegDecoder();

		static List<Image> images = new List<Image>();
		
		public static void Initialize() {
			first.ParseStream(new Uri("http://131.151.19.185:8080/1/stream"));
			second.ParseStream(new Uri("http://131.151.19.185:8080/2/stream"));
			third.ParseStream(new Uri("http://131.151.19.185:8080/3/stream"));

			first.FrameReady += First_FrameReady;
			second.FrameReady += Second_FrameReady;
			third.FrameReady += Third_FrameReady;

			for (int i = 0; i < 3; i++) images.Add(new Image());
		}

		public static void AddSurface(int id, Image surface) {
			images[id - 1] = surface;
		}

		private static void Third_FrameReady(object sender, FrameReadyEventArgs e) {
			images[2].Source = e.BitmapImage;
		}

		private static void Second_FrameReady(object sender, FrameReadyEventArgs e) {
			images[1].Source = e.BitmapImage;
		}

		private static void First_FrameReady(object sender, FrameReadyEventArgs e) {
			images[0].Source = e.BitmapImage;
		}

		/*private static Dictionary<int, MjpegDecoder> decoders = new Dictionary<int, MjpegDecoder>();
		private static Dictionary<MjpegDecoder, Output> outputs = new Dictionary<MjpegDecoder, Output>();

		public static void Initialize() {
			for(int i = 1; i <= 3; i++) {
				MjpegDecoder decode = new MjpegDecoder();
				decode.ParseStream(new Uri($"http://131.151.19.185:8080/{i}/stream"));
				decode.FrameReady += Decode_FrameReady;
				decode.Error += Decode_Error;

				Output use = new Output();
				use.id = i;
				use.surfaces = new List<Image>();
				use.decode = decode;

				decoders.Add(i, decode);
				outputs.Add(decode, use);
			}
		}

		public static void AddSurface(int id, Image surface) {
			MjpegDecoder who = null;

			foreach (Output o in outputs.Values) {
				if (o.id == id) { who = o.decode; break; }
			}

			outputs[who].surfaces.Add(surface);
		}

		private static void Decode_Error(object sender, ErrorEventArgs e) {
			MjpegDecoder who = (MjpegDecoder)sender;

			CommonLog.Instance.Log("Error in camera {0}: {1}", outputs[who].id, e.Message);
		}

		private static void Decode_FrameReady(object sender, FrameReadyEventArgs e) {
			MjpegDecoder who = (MjpegDecoder)sender;
			//CommonLog.Instance.Log("incoming frame for {0}", outputs[who].id);

			foreach(Image surface in outputs[who].surfaces) {
				surface.Source = e.BitmapImage;
			}
		}*/
	}
}
