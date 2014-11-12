namespace RED.Models
{
    public class StateModel
    {
        internal ControlMode _currentControlMode;
        internal bool _networkHasConnection = false;
        internal bool _controllerIsConnected = false;
        internal bool _serverIsRunning = false;
    }

    public enum ControlMode
    {
        Idle,
        Drive,
        RoboticArm,
    }
}
