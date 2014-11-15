namespace RED.ViewModels.ControlCenter
{
    using Caliburn.Micro;
    using Models;
    using Properties;
    using System;
    using System.Linq;

    public class StateViewModel : PropertyChangedBase
    {
        private readonly StateModel _model = new StateModel();
        private readonly ControlCenterViewModel _controlCenter;

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
                return _model._currentControlMode;
            }
            set
            {
                _model._currentControlMode = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => CurrentControlModeDisplay);
            }
        }
        public string CurrentControlModeDisplay
        {
            get
            {
                var mode = _model._currentControlMode;
                return Enum.GetName(typeof(ControlMode), mode);
            }
        }
        public bool NetworkHasConnection
        {
            get
            {
                return _model._networkHasConnection;
            }
            set
            {
                _model._networkHasConnection = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => NetworkConnectionStatus);
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
                return _model._controllerIsConnected;
            }
            set
            {
                _model._controllerIsConnected = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => ControllerConnectionStatus);
            }
        }
        public string ControllerConnectionStatus
        {
            get
            {
                return !ControllerIsConnected ? "Disconnected" : "Connected";
            }
        }

        public bool ServerIsRunning
        {
            get
            {
                return _model._serverIsRunning;
            }
            set
            {
                _model._serverIsRunning = value;
                NotifyOfPropertyChange(() => ServerIsRunning);
            }
        }

        public StateViewModel(ControlCenterViewModel controlCenter)
        {
            _controlCenter = controlCenter;
            CurrentControlMode = ParseEnum<ControlMode>(Settings.Default.DefaultControlMode);
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
            if (CurrentControlMode == ControlMode.Drive)
            {
                // Send zero out command.
            }
            CurrentControlMode = currentIndex == 0
                ? ParseEnum<ControlMode>(controlModes[controlModes.Count - 1])
                : ParseEnum<ControlMode>(controlModes[currentIndex - 1]);
        }

        public void ToggleServer()
        {
            ServerIsRunning = !ServerIsRunning;
            if (ServerIsRunning)
                _controlCenter.TcpAsyncServer.Start();
            else
                _controlCenter.TcpAsyncServer.Stop();
        }

        protected T ParseEnum<T>(string name)
        {
            return (T)Enum.Parse(typeof(T), name, true);
        }

    }
}
