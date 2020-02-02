using Core.Cameras;
using MahApps.Metro.Controls;

namespace RoverAttachmentManager.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowView
    {
        public MainWindowView()
        {
            InitializeComponent();

            CameraMultiplexer.AddSurface(5, Camera5);
            CameraMultiplexer.AddSurface(6, Camera6);
            CameraMultiplexer.AddSurface(7, Camera7);
            CameraMultiplexer.AddSurface(8, Camera8);
        }
    }
}
