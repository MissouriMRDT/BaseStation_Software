namespace RED.Models.ControlCenter
{
    public class StateModel
    {
        internal ControlMode CurrentControlMode;
        internal bool NetworkHasConnection = false;
        internal bool ControllerIsConnected = false;
        internal bool HazelIsReady = false;

        internal bool ServerIsRunning = false;
    }

    public enum ControlMode
    {
        Idle,
        Drive,
        RoboticArm,
    }
}
