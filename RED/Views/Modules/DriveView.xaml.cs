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
				":network-caching=0",
				"--rtsp-caching=0"
				 //"--file-logging", "-vvv", "--extraintf=logger", "--logfile=vlc.log"
			};

			vlcPlayer.MediaPlayer.VlcLibDirectory = vlcLibDirectory;
			vlcPlayer.MediaPlayer.VlcMediaplayerOptions = options;
			vlcPlayer.MediaPlayer.EndInit();
			PlayChannel(2);
		}

		public void PlayChannel(int channel) {
			vlcPlayer.MediaPlayer.Play($"rtsp://admin:Rovin2012@192.168.1.226:554/mpeg4/ch0{channel}/main/av_stream");
		}
	}
}
