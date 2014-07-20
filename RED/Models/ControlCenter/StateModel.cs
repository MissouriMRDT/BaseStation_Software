namespace RED.Models.ControlCenter
{
    public class StateModel
    {
        internal string Title = "State";
        internal bool InUse = false;
        internal bool IsManageable = false;

        internal ControlMode CurrentControlMode;
        internal bool NetworkHasConnection = false;
        internal bool ControllerIsConnected = false;
        internal bool HazelIsReady = false;

        internal int CurrentRedLightValue = 0;
        internal int CurrentGreenLightValue = 0;
        internal int CurrentBlueLightValue = 0;
    }

    public enum ControlMode
    {
        Idle,
        Drive,
        RoboticArm,
        Auxiliary,
        Camera
    }
}
