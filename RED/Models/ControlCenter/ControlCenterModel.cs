namespace RED.Models.ControlCenter
{
    using Contexts;
    using Interfaces;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using ViewModels.Modules;
    using ViewModels.Settings;

    internal class ControlCenterModel
    {
        internal static IEnumerable<IModule> AllModules = new List<IModule>
        {
            new SettingsAppearanceVM(),
            new AuxiliaryVM(),
            new BatteryVM(),
            new InputVM(),
            new PowerboardVM(),
            new NetworkingVM()
        };

        internal static readonly ObservableCollection<ButtonContext> ButtonContexts = new ObservableCollection<ButtonContext>();

        internal string LeftSelection;
        internal string RightSelection;
        internal string TopSelection;
        internal string MiddleSelection;
        internal string BottomSelection;

        internal IModule LeftModule;
        internal IModule RightModule;
        internal IModule TopModule;
        internal IModule MiddleModule;
        internal IModule BottomModule;

        internal IModule SidePanelTopModule;
        internal IModule SidePanelMiddleModule;
        internal IModule SidePanelBottomModule;

        internal string Column1Width = "1*";
        internal string Column3Width = "2*";
        internal string Column5Width = "1*";
        internal string Row1Height = "1*";
        internal string Row3Height = "1*";
        internal string Row5Height = "1*";
    }
}