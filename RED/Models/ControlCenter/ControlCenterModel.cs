namespace RED.Models.ControlCenter
{
	using Contexts;
	using System.Collections.ObjectModel;
	using ViewModels.ControlCenter;

    public class ControlCenterModel
    {
        public SaveModuleStateViewModel SaveModuleState;
        public RemoveModuleStateViewModel RemoveModuleState;

        public StateViewModel StateManager;
        public ConsoleViewModel Console;
        public ModuleManagerViewModel GridManager;
    }
}