namespace RED.Models.Settings
{
    using System.Windows.Media;
    using FirstFloor.ModernUI.Presentation;

    public class SettingsAppearanceModel
    {
        internal string Title = "Appearance Settings";
        internal bool InUse = false;
        internal bool IsManageable = false;

        // 9 accent colors from metro design principles
        internal Color[] AccentColors =
        {
            Color.FromRgb(0x33, 0x99, 0xff),   // blue
            Color.FromRgb(0x00, 0xab, 0xa9),   // teal
            Color.FromRgb(0x33, 0x99, 0x33),   // green
            Color.FromRgb(0x8c, 0xbf, 0x26),   // lime
            Color.FromRgb(0xf0, 0x96, 0x09),   // orange
            Color.FromRgb(0xff, 0x45, 0x00),   // orange red
            Color.FromRgb(0xe5, 0x14, 0x00),   // red
            Color.FromRgb(0xff, 0x00, 0x97),   // magenta
            Color.FromRgb(0xa2, 0x00, 0xff)    // purple            
        };

        internal string FontSmall = "small";
        internal string FontLarge = "large";
        internal Color SelectedAccentColor;
        internal LinkCollection Themes = new LinkCollection();
        internal static Link SelectedTheme;
        internal string SelectedFontSize;
    }
}
