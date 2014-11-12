namespace RED.Models.ControlCenter
{
    using ViewModels.ControlCenter;

    public class ControlCenterModel
    {
        public SaveModuleStateViewModel SaveModuleState;
        public RemoveModuleStateViewModel RemoveModuleState;

        public StateViewModel StateManager;
        public ConsoleViewModel Console;
        public ModuleManagerViewModel GridManager;
        public DataRouter DataRouter;
        public AsyncTcpServer TcpAsyncServer;
    }
}