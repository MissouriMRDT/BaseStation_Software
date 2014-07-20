namespace RED.ViewModels.Settings
{
    using FirstFloor.ModernUI.Presentation;
    using Interfaces;
    using Models.Settings;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Media;

    public class SettingsAppearanceVM : BaseVM, IModule
    {
        private static readonly SettingsAppearanceModel Model = new SettingsAppearanceModel();

        public string Title
        {
            get { return Model.Title; }
        }
        public bool InUse
        {
            get
            {
                return Model.InUse;
            }
            set
            {
                Model.InUse = value;
            }
        }
        public bool IsManageable
        {
            get { return Model.IsManageable; }
        }

        public SettingsAppearanceVM()
        {
            // add the default themes
            Model.Themes.Add(new Link { DisplayName = "dark", Source = AppearanceManager.DarkThemeSource });
            Model.Themes.Add(new Link { DisplayName = "light", Source = AppearanceManager.LightThemeSource });

            SelectedFontSize = AppearanceManager.Current.FontSize == FontSize.Large ? Model.FontLarge : Model.FontSmall;
            SelectedTheme = Themes[0];
            SyncThemeAndColor();

            AppearanceManager.Current.PropertyChanged += OnAppearanceManagerPropertyChanged;
        }

        public LinkCollection Themes
        {
            get { return Model.Themes; }
        }
        public string[] FontSizes
        {
            get { return new[] { Model.FontSmall, Model.FontLarge }; }
        }
        public Color[] AccentColors
        {
            get { return Model.AccentColors; }
        }
        public Link SelectedTheme
        {
            get { return SettingsAppearanceModel.SelectedTheme; }
            set
            {
                if (SettingsAppearanceModel.SelectedTheme == value) return;

                SettingsAppearanceModel.SelectedTheme = value;
                SetField(ref SettingsAppearanceModel.SelectedTheme, value);
                AppearanceManager.Current.ThemeSource = value.Source;
            }
        }
        public string SelectedFontSize
        {
            get { return Model.SelectedFontSize; }
            set
            {
                if (Model.SelectedFontSize == value) return;

                Model.SelectedFontSize = value;
                SetField(ref Model.SelectedFontSize, value);
                AppearanceManager.Current.FontSize = value == Model.FontLarge ? FontSize.Large : FontSize.Small;
            }
        }
        public Color SelectedAccentColor
        {
            get { return Model.SelectedAccentColor; }
            set
            {
                if (Model.SelectedAccentColor == value) return;

                Model.SelectedAccentColor = value;
                SetField(ref Model.SelectedAccentColor, value);
                AppearanceManager.Current.AccentColor = value;
            }
        }

        private void SyncThemeAndColor()
        {
            // synchronizes the selected viewmodel theme with the actual theme used by the appearance manager.
            SelectedTheme = Model.Themes.FirstOrDefault(l => l.Source.Equals(AppearanceManager.Current.ThemeSource));

            // and make sure accent color is up-to-date
            SelectedAccentColor = AppearanceManager.Current.AccentColor;
        }

        private void OnAppearanceManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ThemeSource" || e.PropertyName == "AccentColor")
            {
                SyncThemeAndColor();
            }
        }

        public void TelemetryReceiver<T>(RoverComs.IProtocol<T> message)
        {
            throw new System.NotImplementedException("Appearance Module does not currently receive telemetry data.");
        }
    }
}
