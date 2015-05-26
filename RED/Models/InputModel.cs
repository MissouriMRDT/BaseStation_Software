using RED.Interfaces;
using System.Collections.ObjectModel;

namespace RED.Models
{
    internal class InputModel
    {
        internal int SerialReadSpeed;
        internal int ManualDeadzone;
        internal bool AutoDeadzone = false;

        internal ObservableCollection<IControllerMode> ControllerModes = new ObservableCollection<IControllerMode>();
        internal int CurrentModeIndex;

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
        internal bool ButtonADebounced;
        internal bool ButtonBDebounced;
        internal bool ButtonXDebounced;
        internal bool ButtonYDebounced;
        internal bool ButtonRbDebounced;
        internal bool ButtonLbDebounced;
        internal bool ButtonLsDebounced;
        internal bool ButtonRsDebounced;
        internal bool ButtonStartDebounced;
        internal bool ButtonBackDebounced;
        internal bool DPadL;
        internal bool DPadU;
        internal bool DPadR;
        internal bool DPadD;
    }
}