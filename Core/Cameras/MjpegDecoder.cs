// Code originally from https://github.com/arndre/MjpegDecoder
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Media.Imaging;

namespace Core.Cameras {
	public class NewMjpegDecoder {
		/* 2018-07-16
		 * Bugfixes
		 * Fixed bug in Find Extension
		 */

		/* 2018-02-15
		* 
		* Original article on Coding4Fun: https://channel9.msdn.com/coding4fun/articles/MJPEG-Decoder
		* 
		* Removed preprocessor directives for XNA, WINRT,...
		* Fixed frame drops 
		* Increased performance with dynamic chunksize
		* Replaced boundary with jpeg EOI (https://en.wikipedia.org/wiki/JPEG_File_Interchange_Format)
		* Replaced whole code for image data processing
		* Increased maintainability :)
		* 
		* Arno Dreschnig
		* Alex Faustmann
		*/

		// magic 2 byte for JPEG images
		private readonly byte[] JpegSOI = new byte[] { 0xff, 0xd8 }; // start of image bytes
		private readonly byte[] JpegEOI = new byte[] { 0xff, 0xd9 }; // end of image bytes

		private int ChunkSize = 1024;

		// used to cancel reading the stream
		public bool _streamActive;

		public int ProcessEveryXthFrame { get; set; }

		public int fps;

		// current encoded JPEG image
		public byte[] CurrentFrame { get; private set; }


		// 10 MB
		public const int MAX_BUFFER_SIZE = 1024 * 1024 * 10;

		public SynchronizationContext _context;
		WebRequest request;

		// event to get the buffer above handed to you
		public event EventHandler<FrameReadyEventArgs> FrameReady;
		public event EventHandler<ErrorEventArgs> Error;

		Uri uri;

		public NewMjpegDecoder(int buffer_time = 0) {
			_context = SynchronizationContext.Current;

			ProcessEveryXthFrame = 0;
		}


		public void ParseStream(Uri uri) {
			this.uri = uri;

			ServicePointManager.DefaultConnectionLimit = 15;
			request = (HttpWebRequest)HttpWebRequest.Create(uri);

			_streamActive = true;

			// asynchronously get a response
			request.BeginGetResponse(OnGetResponse, request);

		}
		public void StopStream() {
			_streamActive = false;
			request.Abort();
		}

		private void OnGetResponse(IAsyncResult asyncResult) {
			var request = (HttpWebRequest)asyncResult.AsyncState;

			request.Timeout = 200;

			try {
				var response = (HttpWebResponse)request.EndGetResponse(asyncResult);
				var stream = response.GetResponseStream();
				var binaryReader = new BinaryReader(stream);

				var currentPosition = 0;

				var buffer = new byte[1024];

				while (_streamActive) {
					//read new bytes
					byte[] currentChunk = binaryReader.ReadBytes(ChunkSize);

					if (buffer.Length < currentPosition + currentChunk.Length) {
						if (buffer.Length < MAX_BUFFER_SIZE) {
							// resize buffer to new needed size
							// Console.WriteLine("RESIZE BUFFER " + buffer.Length + " < " + (currentPosition + currentChunk.Length));
							Array.Resize(ref buffer, currentPosition + currentChunk.Length);
						}
						else {
							// hard reset buffer if buffer gets bigger than 10mb
							currentPosition = 0;

							Array.Resize(ref buffer, ChunkSize);

							// log.Debug("Buffer was reset.");
						}
					}

					//copy current bytes to the big byte buffer
					Array.Copy(currentChunk, 0, buffer, currentPosition, currentChunk.Length);

					//increase current position
					currentPosition += currentChunk.Length;

					//find position of magic start of image bytes in big byte buffer
					int soi = buffer.Find(JpegSOI, currentPosition, 0);

					// we have a start of image
					if (soi != -1) {
						// 2018-06-19, Alex
						// Start-At Bug fixed (Alameda Problem)

						//find postion of magic end of image bytes in big byte buffer
						int eoi = buffer.Find(JpegEOI, currentPosition, startAt: soi);

						// we found end of image bytes
						if (eoi != -1) {
							if (eoi > soi) {
								// create new array with image data 
								byte[] img = new byte[eoi - soi + JpegEOI.Length];

								//copy image date from our buffer to image
								Array.Copy(buffer, soi, img, 0, img.Length);

								ProcessFrame(img);

								// calculate remaining buffer size 
								var remainingSize = currentPosition - (eoi + JpegEOI.Length);

								// get position of remaining buffer size
								var endOfCurrentImage = eoi + JpegEOI.Length;

								//copy remaining bytes from current position of buffer to start of buffer
								Array.Copy(buffer, endOfCurrentImage, buffer, 0, remainingSize);

								// reset current position to its actual position in buffer
								currentPosition = remainingSize;

								//recalculate chunk size to avoid too many reads (we thought this is a good idea)
								ChunkSize = Convert.ToInt32(img.Length * 0.5d);
							}
							else {
								// 2018-03-08, Alex
								// Something went wrong. We should skip this frame and move on.
								// @Arno: Maybe find the reason for soi < eoi
								//  log.Debug($"eoi ({eoi}) <= sio ({soi}) error");

								eoi = -1;
								soi = -1;
							}
						}
					}
				}

				//   log.Debug("Loop-Exit.");

				response.Close();

				// log.Debug("Response closed.");
			}
			catch (Exception ex) {
				// log.Error("OnGetResponse-Exception.", ex);

				if (Error != null) {
					_context.Post(delegate { Error(this, new ErrorEventArgs() { Message = ex.Message }); }, null);
				}

				return;
			}
		}


		//DateTime dtLastFrame = DateTime.Now;
		//int counter = 0;

		private void ProcessFrame(byte[] frame) {
			CurrentFrame = frame;

			// 2018-03-07, Alex
			// Check if code is executed in application context (wpf)
			_context.Post(delegate {
				//added try/catch because sometimes jpeg images are corrupted
				try {
					FrameReadyEventArgs args = new FrameReadyEventArgs();
					args.BitmapImage.StreamSource = new MemoryStream(CurrentFrame, 0, CurrentFrame.Length);
					args.Bitmap = Extensions.ConvertBitmapImageToBitmap(args.BitmapImage);

					FrameReady?.Invoke(this, args);
				}
				catch (Exception ex) {
					CommonLog.Instance.Log("MjpegDecoder: error converting image: {0}. Uri: {1}", ex.Message, request.RequestUri);
				}
			}, null);

			// 2018-03-07, Alex
			// Check if code is executed without any context (owin self-host)

		}
	}

	#region extensions

	static class Extensions {
		public static int Find(this byte[] buff, byte[] pattern, int limit = int.MaxValue, int startAt = 0) {
			int patter_match_counter = 0;

			int i = 0;

			for (i = startAt; i < buff.Length && patter_match_counter < pattern.Length && i < limit; i++) {
				if (buff[i] == pattern[patter_match_counter]) {
					patter_match_counter++;
				}
				else {
					patter_match_counter = 0;
				}

			}

			if (patter_match_counter == pattern.Length) {
				return i - pattern.Length; // return _start_ of match pattern
			}
			else {
				return -1;
			}
		}

		public static System.Drawing.Bitmap ConvertBitmapImageToBitmap(BitmapImage bitmapImage) {
			using (MemoryStream outStream = new MemoryStream()) {
				BitmapEncoder enc = new BmpBitmapEncoder();
				enc.Frames.Add(BitmapFrame.Create(bitmapImage));
				enc.Save(outStream);
				System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

				return new System.Drawing.Bitmap(bitmap);
			}
		}

	}

	#endregion

	#region event args

	public class FrameReadyEventArgs : EventArgs {
		public BitmapImage BitmapImage;
		public System.Drawing.Bitmap Bitmap;
	}

	public sealed class ErrorEventArgs : EventArgs {
		public string Message { get; set; }
		public int ErrorCode { get; set; }
	}

	#endregion
}
