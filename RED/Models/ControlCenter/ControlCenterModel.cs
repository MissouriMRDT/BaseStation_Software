namespace RED.Models.ControlCenter
{
<<<<<<< HEAD
    using Contexts;
    using Interfaces;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
<<<<<<< HEAD
    using ViewModels.Modules;
=======
    using ViewModels.Settings;
>>>>>>> 6472c7b... Removed NetworkingVM and AsyncTcpServer

    internal class ControlCenterModel
    {
        internal static IEnumerable<IModule> AllModules = new List<IModule>
        {
<<<<<<< HEAD
            new NetworkingVM()
=======
            new SettingsAppearanceVM(),
>>>>>>> 6472c7b... Removed NetworkingVM and AsyncTcpServer
        };
=======
	using Contexts;
	using Interfaces;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using ViewModels.ControlCenter;

	internal class ControlCenterModel
	{
		internal IEnumerable<IModule> AllModules = new List<IModule>();

		internal readonly ObservableCollection<ButtonContext> ButtonContexts = new ObservableCollection<ButtonContext>();
>>>>>>> d7dd83a... No more statics, all instance-based/DI.

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

		internal StateManager StateManager;
		internal ConsoleVm Console;
		internal ModuleManager ModuleManager;

		internal string Column1Width = "1*";
		internal string Column3Width = "2*";
		internal string Column5Width = "1*";
		internal string Row1Height = "1*";
		internal string Row3Height = "1*";
		internal string Row5Height = "1*";
	}
}