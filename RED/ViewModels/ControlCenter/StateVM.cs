namespace RED.ViewModels.ControlCenter
{
    using Interfaces;
    using Models.ControlCenter;
    using Properties;
    using RED.RoverComs.Rover;
    using RED.ViewModels.Modules;
    using RoverComs;
    using System;
    using System.Linq;

    public class StateVM : BaseVM, IModule
    {
        private static readonly StateModel Model = new StateModel();

        public string Title
        {
            get
            {
                return Model.Title;
            }
        }
        public bool InUse
        {
            get
            {
                return Model.InUse;
            }
            set
            {
                Model.InUse = value;
            }
        }
        public bool IsManageable
        {
            get
            {
                return Model.IsManageable;
            }
        }

        private enum UnderglowLight
        {
            Red,
            Green,
            Blue
        }
        
        public int CurrentRedLightValue
        {
            get
            {
                return Model.CurrentRedLightValue;
            }
            set
            {
                SetField(ref Model.CurrentRedLightValue, value);
                SetUnderglowLight(UnderglowLight.Red, CurrentRedLightValue);
            }
        }
        public int CurrentGreenLightValue
        {
            get
            {
                return Model.CurrentGreenLightValue;
            }
            set
            {
                SetField(ref Model.CurrentGreenLightValue, value);
                SetUnderglowLight(UnderglowLight.Green, CurrentGreenLightValue);
            }
        }
        public int CurrentBlueLightValue
        {
            get
            {
                return Model.CurrentBlueLightValue;
            }
            set
            {
                SetField(ref Model.CurrentBlueLightValue, value);
                SetUnderglowLight(UnderglowLight.Blue, CurrentBlueLightValue);
            }
        }

        public string Version
        {
            get
            {
                return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }
        public ControlMode CurrentControlMode
        {
            get
            {
                return Model.CurrentControlMode;
            }
            set
            {
                SetField(ref Model.CurrentControlMode, value);
                RaisePropertyChanged("CurrentControlModeDisplay");
            }
        }
        public string CurrentControlModeDisplay
        {
            get
            {
                var mode = Model.CurrentControlMode;
                return Enum.GetName(typeof(ControlMode), mode);
            }
        }
        public bool NetworkHasConnection
        {
            get
            {
                return Model.NetworkHasConnection;
            }
            set
            {
                SetField(ref Model.NetworkHasConnection, value);
                RaisePropertyChanged("NetworkConnectionStatus");
            }
        }
        public string NetworkConnectionStatus
        {
            get
            {
                return !NetworkHasConnection ? "Disconnected" : "Connected";
            }
        }
        public bool ControllerIsConnected
        {
            get
            {
                return Model.ControllerIsConnected;
            }
            set
            {
                SetField(ref Model.ControllerIsConnected, value);
                RaisePropertyChanged("ConnectionStatus");
                RaisePropertyChanged("ControllerConnectionStatus");
            }
        }
        public string ControllerConnectionStatus
        {
            get
            {
                return !ControllerIsConnected ? "Disconnected" : "Connected";
            }
        }
        public bool HazelIsReady
        {
            get
            {
                return Model.HazelIsReady;
            }
            set
            {
                SetField(ref Model.HazelIsReady, value);
            }
        }

        public StateVM()
        {
            CurrentControlMode = ParseEnum<ControlMode>(Settings.Default.DefaultControlMode);
        }

        private void SetUnderglowLight(UnderglowLight light, int value)
        {
            switch(light)
            {
                case UnderglowLight.Red:
                    GetModuleViewModel<NetworkingVM>().SendProtocol(new Protocol<int>((int)Auxiliary.CommandId.RedLight, value));
                    break;
                case UnderglowLight.Green:
                    GetModuleViewModel<NetworkingVM>().SendProtocol(new Protocol<int>((int)Auxiliary.CommandId.GreenLight, value));
                    break;
                case UnderglowLight.Blue:
                    GetModuleViewModel<NetworkingVM>().SendProtocol(new Protocol<int>((int)Auxiliary.CommandId.BlueLight, value));
                    break;
            }
        }
        
        public void NextControlMode()
        {
            var controlModes = Enum.GetNames(typeof(ControlMode)).ToList();
            var currentIndex = controlModes.IndexOf(CurrentControlModeDisplay);
            if (CurrentControlMode == ControlMode.Drive)
            {
                // Send zero out command.
            }
            CurrentControlMode = currentIndex == controlModes.Count - 1
                ? ParseEnum<ControlMode>(controlModes[0])
                : ParseEnum<ControlMode>(controlModes[currentIndex + 1]);
        }
        public void PreviousControlMode()
        {
            var controlModes = Enum.GetNames(typeof(ControlMode)).ToList();
            var currentIndex = controlModes.IndexOf(CurrentControlModeDisplay);
            if(CurrentControlMode == ControlMode.Drive)
            {
                // Send zero out command.
            }
            CurrentControlMode = currentIndex == 0
                ? ParseEnum<ControlMode>(controlModes[controlModes.Count - 1])
                : ParseEnum<ControlMode>(controlModes[currentIndex - 1]);
        }
        
        public void TelemetryReceiver<T>(IProtocol<T> message)
        {
            throw new NotImplementedException("State Module does not currently receive telemetry.");
        }
    }
}
