namespace RED.Models.Modules
{
    internal class InputModel
    {
        internal string Title = "Input and Command Systems";
        internal bool InUse = false;
        internal bool IsManageable = true;

        internal bool Connected;
        internal string ConnectionStatus = "Disconnected";
        internal float JoyStick1X;
        internal float JoyStick1Y;
        internal float JoyStick2X;
        internal float JoyStick2Y;
        internal float LeftTrigger;
        internal float RightTrigger;
        internal bool ButtonA;
        internal bool ButtonB;
        internal bool ButtonX;
        internal bool ButtonY;
        internal bool ButtonRb;
        internal bool ButtonLb;
        internal bool ButtonLs;
        internal bool ButtonRs;
        internal bool ButtonStart;
        internal bool ButtonBack;
        internal bool DPadL;
        internal bool DPadU;
        internal bool DPadR;
        internal bool DPadD;

        internal int CurrentRawControllerSpeedLeft;
        internal int CurrentRawControllerSpeedRight;
        internal int SpeedLeft;
        internal int SpeedRight;
    }
}
