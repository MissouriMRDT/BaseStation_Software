namespace RED.Models
{
    using ViewModels.ControlCenter;

    public class ControlCenterModel
    {
        internal SaveModuleStateViewModel _saveModuleState;
        internal RemoveModuleStateViewModel _removeModuleState;

        internal StateViewModel _stateManager;
        internal ConsoleViewModel _console;
        internal ModuleManagerViewModel _gridManager;
        internal DataRouter _dataRouter;
        internal AsyncTcpServerViewModel _tcpAsyncServer;
    }
}