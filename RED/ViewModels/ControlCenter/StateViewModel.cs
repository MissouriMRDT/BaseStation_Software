namespace RED.ViewModels.ControlCenter
{
    using Caliburn.Micro;
    using Models.ControlCenter;
	using Properties;
	using System;
	using System.Linq;

	public class StateViewModel : PropertyChangedBase
	{
		private readonly StateModel _model = new StateModel();

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
				return _model.CurrentControlMode;
			}
			set
			{
				_model.CurrentControlMode = value;
				NotifyOfPropertyChange();
				NotifyOfPropertyChange(() => CurrentControlModeDisplay);
			}
		}
		public string CurrentControlModeDisplay
		{
			get
			{
				var mode = _model.CurrentControlMode;
				return Enum.GetName(typeof(ControlMode), mode);
			}
		}
		public bool NetworkHasConnection
		{
			get
			{
				return _model.NetworkHasConnection;
			}
			set
			{
				_model.NetworkHasConnection = value;
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
				return _model.ControllerIsConnected;
			}
			set
			{
				_model.ControllerIsConnected = value;
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
		public bool HazelIsReady
		{
			get
			{
				return _model.HazelIsReady;
			}
			set
			{
				_model.HazelIsReady = value;
				NotifyOfPropertyChange();
			}
		}

		public StateViewModel()
		{
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

        protected T ParseEnum<T>(string name)
        {
            return (T)Enum.Parse(typeof(T), name, true);
        }

	}
}
