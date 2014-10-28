namespace RED.Models.ControlCenter
{
    using RED.Contexts;
    using RED.ViewModels.ControlCenter;
    using System.Collections.ObjectModel;

    public class ControlCenterModel
    {
        public SaveModuleStateViewModel SaveModuleState;
        public RemoveModuleStateViewModel RemoveModuleState;

        public readonly ObservableCollection<ButtonContext> ButtonContexts = new ObservableCollection<ButtonContext>();

        public StateViewModel StateManager;
        public ConsoleViewModel Console;
        public ModuleManagerViewModel GridManager;
        public DataRouterVM DataRouter;
        public TCPAsyncServerVM TcpAsyncServer;
    }
}