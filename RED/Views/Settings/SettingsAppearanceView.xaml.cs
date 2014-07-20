namespace RED.Views.Settings
{
    using System.Linq;
    using ViewModels.ControlCenter;

    public partial class SettingsAppearanceView
    {
        public SettingsAppearanceView()
        {
            InitializeComponent();
            DataContext = ControlCenterVM.UnmanagedModules.Single(m => m.Title == "Appearance Settings");
        }
    }
}
