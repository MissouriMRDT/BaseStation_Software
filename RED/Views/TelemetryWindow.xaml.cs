namespace RED
{
    using FirstFloor.ModernUI.Presentation;
    using System.Windows.Media;

    public partial class TelemetryWindow
    {
        public TelemetryWindow()
        {
            InitializeComponent();
            AppearanceManager.Current.AccentColor = Color.FromRgb(0xe5, 0x14, 0x00);  
        }
    }
}
