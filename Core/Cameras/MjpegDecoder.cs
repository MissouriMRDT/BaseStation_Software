// Code originally from https://github.com/arndre/MjpegDecoder
// This code is MIT licensed

using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Media.Imaging;

namespace Core.Cameras {
	public class MjpegDecoder {
		/* 2019-11-18
		 * Added support for BitmapImage
		 * Added timeout to initial request
		 * Added Guid to uniquely identify this decoder instance
		 * 
		 * Matt Montgomery
		 */

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

		// magic bytes for JPEG images
		private readonly byte[] JpegSOI = new byte[] { 0xff, 0xd8 };
		private readonly byte[] JpegEOI = new byte[] { 0xff, 0xd9 };

		private int ChunkSize = 1024;

		// used to cancel reading the stream
		public bool _streamActive;

		// current encoded JPEG image
		public byte[] CurrentFrame { get; private set; }

		// 10 MB
		public const int MAX_BUFFER_SIZE = 10 * 1024 * 1024;

		public SynchronizationContext _context;
		WebRequest request;

		public event EventHandler<FrameReadyEventArgs> FrameReady;
		public event EventHandler<ErrorEventArgs> Error;

		public Guid UUID { get; private set; }

		public MjpegDecoder(int buffer_time = 0) {
			_context = SynchronizationContext.Current;

			UUID = Guid.NewGuid();
		}
		
		public void ParseStream(Uri uri) {
			ServicePointManager.DefaultConnectionLimit = 15;
			request = (HttpWebRequest)HttpWebRequest.Create(uri);
			request.Timeout = 5000;

			_streamActive = true;

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
							Array.Resize(ref buffer, currentPosition + currentChunk.Length);
						}
						else {
							// hard reset buffer if buffer gets bigger than 10mb
							currentPosition = 0;

							Array.Resize(ref buffer, ChunkSize);
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

								eoi = -1;
								soi = -1;
							}
						}
					}
				}

				response.Close();
			}
			catch (Exception ex) {
				if (Error != null) {
					_context.Post(delegate { Error(this, new ErrorEventArgs() { Message = ex.Message }); }, null);
				}

				return;
			}
		}

		private void ProcessFrame(byte[] frame) {
			CurrentFrame = frame;

			// 2018-03-07, Alex
			// Check if code is executed in application context (wpf)
			_context.Post(delegate {
				try {
					FrameReadyEventArgs args = new FrameReadyEventArgs();
					using(MemoryStream ms = new MemoryStream(CurrentFrame, 0, CurrentFrame.Length)) {
						BitmapImage img = new BitmapImage();

						img.BeginInit();
						img.CacheOption = BitmapCacheOption.OnLoad;
						img.StreamSource = ms;
						img.EndInit();

						args.BitmapImage = img;
					}
				
					FrameReady?.Invoke(this, args);
				}
				catch (Exception ex) {
					CommonLog.Instance.Log("MjpegDecoder: error converting image: {0}. Uri: {1}. Trace: {2}", ex.Message, request.RequestUri, ex.StackTrace);
				}
			}, null);
		}
	}

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

	}

	public class FrameReadyEventArgs : EventArgs {
		public BitmapImage BitmapImage;
	}

	public sealed class ErrorEventArgs : EventArgs {
		public string Message { get; set; }
		public int ErrorCode { get; set; }
	}
}
