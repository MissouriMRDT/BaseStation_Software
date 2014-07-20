namespace RED.ViewModels.Modules
{
    using System;
    using ControlCenter;
    using Interfaces;
    using Models.Modules;
    using RoverComs;
    using RoverComs.Rover;
    using System.Windows.Input;
    using FirstFloor.ModernUI.Presentation;
    using RED.Contexts;

    public class AuxiliaryVM : BaseVM, IModule
    {
        private static readonly AuxiliaryModel Model = new AuxiliaryModel();
        private const int OPEN = 0;
        private const int CLOSE = 1;
        private const int OFF = 0;
        private const int ON = 1;
        private const int FORWARD = 1;
        private const int BACK = 1;

        public string Title
        {
            get { return Model.Title; }
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

        public ICommand ToggleGpsTelemetryOnCommand { get; private set; }
        public ICommand ToggleGpsTelemetryOffCommand { get; private set; }
        public ICommand ToggleDrillDirectionForwardCommand { get; private set; }
        public ICommand ToggleDrillDirectionBackCommand { get; private set; }
        public ICommand ToggleHeaterPowerOnCommand { get; private set; }
        public ICommand ToggleHeaterPowerOffCommand { get; private set; }
        public ICommand ToggleThermalReadingsOnCommand { get; private set; }
        public ICommand ToggleThermalReadingsOffCommand { get; private set; }
        public ICommand ToggleSensorPowerOnCommand { get; private set; }
        public ICommand ToggleSensorPowerOffCommand { get; private set; }
        public ICommand ToggleGasReadingsOnCommand { get; private set; }
        public ICommand ToggleGasReadingsOffCommand { get; private set; }
        public ICommand ToggleLight395OnCommand { get; private set; }
        public ICommand ToggleLight395OffCommand { get; private set; }
        public ICommand ToggleLight440OnCommand { get; private set; }
        public ICommand ToggleLight440OffCommand { get; private set; }
        public ICommand ToggleSampleBayDoorOpenCommand { get; private set; }
        public ICommand ToggleSampleBayDoorCloseCommand { get; private set; }
        public ICommand ToggleDrillSensorTelemetryOnCommand { get; private set; }
        public ICommand ToggleDrillSensorTelemetryOffCommand { get; private set; }

        public TelemetryContext<string> GpsFixedDisplay
        {
            get { return new TelemetryContext<string>(GpsFixed.ReceivedOn, GpsFixed.Value ? "Acquired" : "Searching..."); }
        }
        public TelemetryContext<bool> GpsFixed
        {
            get
            {
                return Model.GpsFixed;
            }
            set
            {
                SetField(ref Model.GpsFixed, value);
                RaisePropertyChanged("GpsFixedDisplay");
            }
        }
        public TelemetryContext<string> GpsLatitude
        {
            get
            {
                return Model.GpsLatitude;
            }
            set
            {
                SetField(ref Model.GpsLatitude, value);
            }
        }
        public TelemetryContext<string> GpsLongitude
        {
            get
            {
                return Model.GpsLongitude;
            }
            set
            {
                SetField(ref Model.GpsLongitude, value);
            }
        }
        public TelemetryContext<string> GpsAltitude
        {
            get
            {
                return Model.GpsAltitude;
            }
            set
            {
                SetField(ref Model.GpsAltitude, value);
            }
        }
        public TelemetryContext<int> GpsSatelliteCount
        {
            get
            {
                return Model.GpsSatelliteCount;
            }
            set
            {
                SetField(ref Model.GpsSatelliteCount, value);
            }
        }
        
        public int CurrentDrillSpeed
        {
            get
            {
                return Model.CurrentDrillSpeed;
            }
            set
            {
                SetField(ref Model.CurrentDrillSpeed, value);
                SetDrillSpeed(value);
            }
        }

        public TelemetryContext<int> DrillHydrogenReading
        {
            get
            {
                return Model.DrillHydrogenReading;
            }
            set
            {
                SetField(ref Model.DrillHydrogenReading, value);
            }
        }
        public TelemetryContext<int> DrillMethaneReading
        {
            get
            {
                return Model.DrillMethaneReading;
            }
            set
            {
                SetField(ref Model.DrillMethaneReading, value);
            }
        }
        public TelemetryContext<int> DrillAmmoniaReading
        {
            get
            {
                return Model.DrillAmmoniaReading;
            }
            set
            {
                SetField(ref Model.DrillAmmoniaReading, value);
            }
        }
        public TelemetryContext<double> DrillTemperature
        {
            get
            {
                return Model.DrillTemperature;
            }
            set
            {
                SetField(ref Model.DrillTemperature, value);
            }
        }
        public TelemetryContext<int> DrillActualSpeed
        {
            get
            {
                return Model.DrillActualSpeed;
            }
            set
            {
                SetField(ref Model.DrillActualSpeed, value);
            }
        }

        public AuxiliaryVM()
            : base(Model.Title)
        {
            GpsFixed = new TelemetryContext<bool>(DateTime.Now, false);

            ToggleGpsTelemetryOnCommand = new RelayCommand(c => ToggleGpsTelemetry(ON));
            ToggleGpsTelemetryOffCommand = new RelayCommand(c => ToggleGpsTelemetry(OFF));
            ToggleDrillDirectionForwardCommand = new RelayCommand(c => ToggleDrillDirection(FORWARD));
            ToggleDrillDirectionBackCommand = new RelayCommand(c => ToggleDrillDirection(BACK));
            ToggleHeaterPowerOnCommand = new RelayCommand(c => ToggleHeaterPower(ON));
            ToggleHeaterPowerOffCommand = new RelayCommand(c => ToggleHeaterPower(OFF));
            ToggleThermalReadingsOnCommand = new RelayCommand(c => ToggleThermalReadings(ON));
            ToggleThermalReadingsOffCommand = new RelayCommand(c => ToggleThermalReadings(OFF));
            ToggleSensorPowerOnCommand = new RelayCommand(c => ToggleSensorPower(ON));
            ToggleSensorPowerOffCommand = new RelayCommand(c => ToggleSensorPower(OFF));
            ToggleGasReadingsOnCommand = new RelayCommand(c => ToggleGasReadings(ON));
            ToggleGasReadingsOffCommand = new RelayCommand(c => ToggleGasReadings(OFF));
            ToggleLight395OnCommand = new RelayCommand(c => ToggleLight395(ON));
            ToggleLight395OffCommand = new RelayCommand(c => ToggleLight395(OFF));
            ToggleLight440OnCommand = new RelayCommand(c => ToggleLight440(ON));
            ToggleLight440OffCommand = new RelayCommand(c => ToggleLight440(OFF));
            ToggleSampleBayDoorOpenCommand = new RelayCommand(c => ToggleSampleBayDoor(OPEN));
            ToggleSampleBayDoorCloseCommand = new RelayCommand(c => ToggleSampleBayDoor(CLOSE));
            ToggleDrillSensorTelemetryOnCommand = new RelayCommand(c => ToggleDrillSensorTelemetry(ON));
            ToggleDrillSensorTelemetryOffCommand = new RelayCommand(c => ToggleDrillSensorTelemetry(OFF));
        }

        private void ToggleGpsTelemetry(int value)
        {
            GetModuleViewModel<NetworkingVM>().SendProtocol(new Protocol<int>((int)Motherboard.CommandId.ToggleGpsTelemetry, value));
        }
        private void SetDrillSpeed(int value)
        {
            GetModuleViewModel<NetworkingVM>().SendProtocol(new Protocol<int>((int)Auxiliary.CommandId.DrillSpeed, value));
        }
        private void ToggleDrillDirection(int value)
        {
            GetModuleViewModel<NetworkingVM>().SendProtocol(new Protocol<int>((int)Auxiliary.CommandId.DrillDirection, value));
        }
        private void ToggleHeaterPower(int value)
        {
            GetModuleViewModel<NetworkingVM>().SendProtocol(new Protocol<int>((int)Auxiliary.CommandId.HeaterPower, value));
        }
        private void ToggleThermalReadings(int value)
        {
            GetModuleViewModel<NetworkingVM>().SendProtocol(new Protocol<int>((int)Auxiliary.CommandId.ThermalReadings, value));
        }
        private void ToggleSensorPower(int value)
        {
            GetModuleViewModel<NetworkingVM>().SendProtocol(new Protocol<int>((int)Auxiliary.CommandId.SensorPower, value));
        }
        private void ToggleGasReadings(int value)
        {
            GetModuleViewModel<NetworkingVM>().SendProtocol(new Protocol<int>((int)Auxiliary.CommandId.GasReadings, value));
        }
        private void ToggleLight395(int value)
        {
            GetModuleViewModel<NetworkingVM>().SendProtocol(new Protocol<int>((int)Auxiliary.CommandId.Light395, value));
        }
        private void ToggleLight440(int value)
        {
            GetModuleViewModel<NetworkingVM>().SendProtocol(new Protocol<int>((int)Auxiliary.CommandId.Light440, value));
        }
        private void ToggleSampleBayDoor(int value)
        {
            GetModuleViewModel<NetworkingVM>().SendProtocol(new Protocol<int>((int)Auxiliary.CommandId.Door, value == 1 ? 255 : 0));
        }
        private void ToggleDrillSensorTelemetry(int value)
        {
            GetModuleViewModel<NetworkingVM>().SendProtocol(new Protocol<int>((int)Auxiliary.CommandId.ToggleDrillSensorTelemetry, value));
        }

        public void TelemetryReceiver<T>(IProtocol<T> message)
        {
            try
            {
                switch ((Auxiliary.TelemetryId)message.Id)
                {
                    case Auxiliary.TelemetryId.DrillHydrogenReading:
                        DrillHydrogenReading = new TelemetryContext<int>(int.Parse(message.Value.ToString()));
                        return;
                    case Auxiliary.TelemetryId.DrillMethaneReading:
                        DrillMethaneReading = new TelemetryContext<int>(int.Parse(message.Value.ToString()));
                        return;
                    case Auxiliary.TelemetryId.DrillAmmoniaReading:
                        DrillAmmoniaReading = new TelemetryContext<int>(int.Parse(message.Value.ToString()));
                        return;
                    case Auxiliary.TelemetryId.DrillTemperature:
                        DrillTemperature = new TelemetryContext<double>(double.Parse(message.Value.ToString()));
                        return;
                    case Auxiliary.TelemetryId.DrillActualSpeed:
                        DrillActualSpeed = new TelemetryContext<int>(int.Parse(message.Value.ToString()));
                        return;
                }

                switch ((Motherboard.TelemetryId)message.Id)
                {
                    case Motherboard.TelemetryId.GpsFix:
                        GpsFixed = new TelemetryContext<bool>(int.Parse(message.Value.ToString()) == 1 ? true : false);
                        break;
                    case Motherboard.TelemetryId.GpsLatitude:
                        var latitudeText = message.Value.ToString();
                        var latitude = float.Parse(latitudeText.Substring(0, latitudeText.Length - 1));
                        latitude *= latitudeText[latitudeText.Length - 1] == 'W' || latitudeText[latitudeText.Length - 1] == 'S' ? -1 : 1;
                        GpsLatitude = new TelemetryContext<string>(ConvertDegreeMinuteToDecimalDegree(latitude).ToString());
                        break;
                    case Motherboard.TelemetryId.GpsLongitude:
                        var longitudeText = message.Value.ToString();
                        var longitude = float.Parse(longitudeText.Substring(0, longitudeText.Length - 1));
                        longitude *= longitudeText[longitudeText.Length - 1] == 'W' || longitudeText[longitudeText.Length - 1] == 'S' ? -1 : 1;
                        GpsLongitude = new TelemetryContext<string>(ConvertDegreeMinuteToDecimalDegree(longitude).ToString());
                        break;
                    case Motherboard.TelemetryId.GpsAltitude:
                        GpsAltitude = new TelemetryContext<string>(message.Value.ToString() + "m");
                        break;
                    case Motherboard.TelemetryId.GpsSatelliteCount:
                        GpsSatelliteCount = new TelemetryContext<int>(int.Parse(message.Value.ToString()));
                        break;
                    default:
                        throw new Exception(Title + " telemetry receiving failed.");
                }
            }
            catch (Exception e)
            {
                ControlCenterVM.ConsoleVM.TelemetryReceiver(new Protocol<string>(e.Message));
            }
        }

        //  <summary>
        //  Converts latitude/longitude from Adafruit degree-minute format to decimal-degrees
        //  </summary>
        private double ConvertDegreeMinuteToDecimalDegree(float degreeMinute)
        {
            double minutes = 0.0;
            double decimalDegree = 0.0;

            minutes = degreeMinute % 100;

            degreeMinute = (int)(degreeMinute / 100);
            decimalDegree = degreeMinute + (minutes / 60);
            return decimalDegree;
        }
    }
}
