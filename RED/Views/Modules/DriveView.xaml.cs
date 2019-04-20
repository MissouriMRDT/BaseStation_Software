using System.IO;
using System.Windows.Controls;

namespace RED.Views.Modules
{
    /// <summary>
    /// Interaction logic for DriveView.xaml
    /// </summary>
    public partial class DriveView : UserControl
    {
        public DriveView()
        {
            InitializeComponent();
			SetupVLC();
        }

		internal void SetupVLC() {
			var vlcLibDirectory = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "libvlc", System.IntPtr.Size == 4 ? "win-x86" : "win-x64"));

			var options = new string[]
			{
                // the network caching value is a milliseconds value that determines the amount of video data to store and decode
                // if this value is too low, the buffer will not contain enough video data to allow the underlying video library to decode and display
                //     *anything* (not even corrupt frames).
                // in order to tune this value, slowly de-increment this value until messages about the picture being late stop being logged.
				":network-caching=300",
				":realrtsp-caching=100"
			};

			vlcPlayer.MediaPlayer.VlcLibDirectory = vlcLibDirectory;
			vlcPlayer.MediaPlayer.VlcMediaplayerOptions = options;
			vlcPlayer.MediaPlayer.EndInit();
			PlayChannel(1);
		}

		public void PlayChannel(int channel) {
			vlcPlayer.MediaPlayer.Play($"rtsp://admin:Rovin2012@192.168.1.226:554/mpeg4/ch0{channel}/main/av_stream");
		}
	}
}
