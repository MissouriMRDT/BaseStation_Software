namespace RED.ViewModels.Modules
{
    using Contexts;
    using ControlCenter;
    using Interfaces;
    using Models.Modules;
    using RoverComs;
    using RoverComs.Rover;
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Threading;

    public class BatteryVM : BaseVM, IModule
    {
        private static readonly BatteryModel Model = new BatteryModel();
        internal const int MAX_GRAPH_SIZE = 10;

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
        
        public ObservableCollection<BarContext> MainBatteryCurrentContexts
        {
            get
            {
                return Model.MainBatteryCurrentContexts;
            }
        }
        public ObservableCollection<BarContext> MainBatteryVoltageContexts
        {
            get
            {
                return Model.MainBatteryVoltageContexts;
            }
        }
        public ObservableCollection<BarContext> Voltage1Contexts
        {
            get
            {
                return Model.Voltage1Contexts;
            }
        }
        public ObservableCollection<BarContext> Voltage2Contexts
        {
            get
            {
                return Model.Voltage2Contexts;
            }
        }
        public ObservableCollection<BarContext> Voltage3Contexts
        {
            get
            {
                return Model.Voltage3Contexts;
            }
        }
        public ObservableCollection<BarContext> Voltage4Contexts
        {
            get
            {
                return Model.Voltage4Contexts;
            }
        }
        public ObservableCollection<BarContext> Voltage5Contexts
        {
            get
            {
                return Model.Voltage5Contexts;
            }
        }
        public ObservableCollection<BarContext> Voltage6Contexts
        {
            get
            {
                return Model.Voltage6Contexts;
            }
        }
        public ObservableCollection<BarContext> Voltage7Contexts
        {
            get
            {
                return Model.Voltage7Contexts;
            }
        }
        public ObservableCollection<BarContext> Temperature1Contexts
        {
            get
            {
                return Model.Temperature1Contexts;
            }
        }
        public ObservableCollection<BarContext> Temperature2Contexts
        {
            get
            {
                return Model.Temperature2Contexts;
            }
        }
        public ObservableCollection<BarContext> Temperature3Contexts
        {
            get
            {
                return Model.Temperature3Contexts;
            }
        }
        public ObservableCollection<BarContext> Temperature4Contexts
        {
            get
            {
                return Model.Temperature4Contexts;
            }
        }
        public ObservableCollection<BarContext> Temperature5Contexts
        {
            get
            {
                return Model.Temperature5Contexts;
            }
        }
        public ObservableCollection<BarContext> Temperature6Contexts
        {
            get
            {
                return Model.Temperature6Contexts;
            }
        }
        public ObservableCollection<BarContext> Temperature7Contexts
        {
            get
            {
                return Model.Temperature7Contexts;
            }
        }

        public double MainBatteryCurrent
        {
            get
            {
                return (Model.MainBatteryCurrent - MainBatteryCurrentOffset) / MainBatteryCurrentConversionValue;
            }
            set
            {
                SetField(ref Model.MainBatteryCurrent, value);
            }
        }
        public double MainBatteryVoltage
        {
            get
            {
                return Model.MainBatteryVoltage;
            }
            set
            {
                SetField(ref Model.MainBatteryVoltage, value);
            }
        }
        public double Voltage1
        {
            get
            {
                return Model.Voltage1 * VoltageConversionValue;
            }
            set
            {
                SetField(ref Model.Voltage1, value);
            }
        }
        public double Voltage2
        {
            get
            {
                return Model.Voltage2 * VoltageConversionValue;
            }
            set
            {
                SetField(ref Model.Voltage2, value);
            }
        }
        public double Voltage3
        {
            get
            {
                return Model.Voltage3 * VoltageConversionValue;
            }
            set
            {
                SetField(ref Model.Voltage3, value);
            }
        }
        public double Voltage4
        {
            get
            {
                return Model.Voltage4 * VoltageConversionValue;
            }
            set
            {
                SetField(ref Model.Voltage4, value);
            }
        }
        public double Voltage5
        {
            get
            {
                return Model.Voltage5 * VoltageConversionValue;
            }
            set
            {
                SetField(ref Model.Voltage5, value);
            }
        }
        public double Voltage6
        {
            get
            {
                return Model.Voltage6 * VoltageConversionValue;
            }
            set
            {
                SetField(ref Model.Voltage6, value);
            }
        }
        public double Voltage7
        {
            get
            {
                return Model.Voltage7 * VoltageConversionValue;
            }
            set
            {
                SetField(ref Model.Voltage7, value);
            }
        }
        public double Temperature1
        {
            get
            {
                return Model.Temperature1 * TemperatureConversionValue;
            }
            set
            {
                SetField(ref Model.Temperature1, value);
            }
        }
        public double Temperature2
        {
            get
            {
                return Model.Temperature2 * TemperatureConversionValue;
            }
            set
            {
                SetField(ref Model.Temperature2, value);
            }
        }
        public double Temperature3
        {
            get
            {
                return Model.Temperature3 * TemperatureConversionValue;
            }
            set
            {
                SetField(ref Model.Temperature3, value);
            }
        }
        public double Temperature4
        {
            get
            {
                return Model.Temperature4 * TemperatureConversionValue;
            }
            set
            {
                SetField(ref Model.Temperature4, value);
            }
        }
        public double Temperature5
        {
            get
            {
                return Model.Temperature5 * TemperatureConversionValue;
            }
            set
            {
                SetField(ref Model.Temperature5, value);
            }
        }
        public double Temperature6
        {
            get
            {
                return Model.Temperature6 * TemperatureConversionValue;
            }
            set
            {
                SetField(ref Model.Temperature6, value);
            }
        }
        public double Temperature7
        {
            get
            {
                return Model.Temperature7 * TemperatureConversionValue;
            }
            set
            {
                SetField(ref Model.Temperature7, value);
            }
        }

        public BatteryVM() : base(Model.Title)
        {

        }

        public void TelemetryReceiver<T>(IProtocol<T> message)
        {
            try
            {
                switch ((Bms.TelemetryId)message.Id)
                {
                    case Bms.TelemetryId.MainBatteryCurrent:
                        MainBatteryCurrent = double.Parse(message.Value.ToString());
                        if (MainBatteryCurrentContexts.Count >= MAX_GRAPH_SIZE)
                            Application.Current.Dispatcher.Invoke(() => MainBatteryCurrentContexts.RemoveAt(MainBatteryCurrentContexts.Count - 1), DispatcherPriority.ContextIdle);
                        Application.Current.Dispatcher.Invoke(() => MainBatteryCurrentContexts.Insert(0, new BarContext(DateTime.Now, MainBatteryCurrent)), DispatcherPriority.ContextIdle);
                        break;
                    case Bms.TelemetryId.MainBatteryVoltage:
                        MainBatteryVoltage = Voltage1 + Voltage2 + Voltage3 + Voltage4 + Voltage5 + Voltage6 + Voltage7;
                        // MainBatteryVoltage = double.Parse(message.Value.ToString());
                        if (MainBatteryVoltageContexts.Count >= MAX_GRAPH_SIZE)
                            Application.Current.Dispatcher.Invoke(() => MainBatteryVoltageContexts.RemoveAt(MainBatteryVoltageContexts.Count - 1), DispatcherPriority.ContextIdle);
                        Application.Current.Dispatcher.Invoke(() => MainBatteryVoltageContexts.Insert(0, new BarContext(DateTime.Now, MainBatteryVoltage)), DispatcherPriority.ContextIdle);
                        break;
                    case Bms.TelemetryId.Temperature1:
                        Temperature1 = double.Parse(message.Value.ToString());
                        if (Temperature1Contexts.Count >= MAX_GRAPH_SIZE)
                            Application.Current.Dispatcher.Invoke(() => Temperature1Contexts.RemoveAt(Temperature1Contexts.Count - 1), DispatcherPriority.ContextIdle);
                        Application.Current.Dispatcher.Invoke(() => Temperature1Contexts.Insert(0, new BarContext(DateTime.Now, Temperature1)), DispatcherPriority.ContextIdle);
                        break;
                    case Bms.TelemetryId.Temperature2:
                        Temperature2 = double.Parse(message.Value.ToString());
                        if (Temperature2Contexts.Count >= MAX_GRAPH_SIZE)
                            Application.Current.Dispatcher.Invoke(() => Temperature2Contexts.RemoveAt(Temperature2Contexts.Count - 1), DispatcherPriority.ContextIdle);
                        Application.Current.Dispatcher.Invoke(() => Temperature2Contexts.Insert(0, new BarContext(DateTime.Now, Temperature2)), DispatcherPriority.ContextIdle);
                        break;
                    case Bms.TelemetryId.Temperature3:
                        Temperature3 = double.Parse(message.Value.ToString());
                        if (Temperature3Contexts.Count >= MAX_GRAPH_SIZE)
                            Application.Current.Dispatcher.Invoke(() => Temperature3Contexts.RemoveAt(Temperature3Contexts.Count - 1), DispatcherPriority.ContextIdle);
                        Application.Current.Dispatcher.Invoke(() => Temperature3Contexts.Insert(0, new BarContext(DateTime.Now, Temperature3)), DispatcherPriority.ContextIdle);
                        break;
                    case Bms.TelemetryId.Temperature4:
                        Temperature4 = double.Parse(message.Value.ToString());
                        if (Temperature4Contexts.Count >= MAX_GRAPH_SIZE)
                            Application.Current.Dispatcher.Invoke(() => Temperature4Contexts.RemoveAt(Temperature4Contexts.Count - 1), DispatcherPriority.ContextIdle);
                        Application.Current.Dispatcher.Invoke(() => Temperature4Contexts.Insert(0, new BarContext(DateTime.Now, Temperature4)), DispatcherPriority.ContextIdle);
                        break;
                    case Bms.TelemetryId.Temperature5:
                        Temperature5 = double.Parse(message.Value.ToString());
                        if (Temperature5Contexts.Count >= MAX_GRAPH_SIZE)
                            Application.Current.Dispatcher.Invoke(() => Temperature5Contexts.RemoveAt(Temperature5Contexts.Count - 1), DispatcherPriority.ContextIdle);
                        Application.Current.Dispatcher.Invoke(() => Temperature5Contexts.Insert(0, new BarContext(DateTime.Now, Temperature5)), DispatcherPriority.ContextIdle);
                        break;
                    case Bms.TelemetryId.Temperature6:
                        Temperature6 = double.Parse(message.Value.ToString());
                        if (Temperature6Contexts.Count >= MAX_GRAPH_SIZE)
                            Application.Current.Dispatcher.Invoke(() => Temperature6Contexts.RemoveAt(Temperature6Contexts.Count - 1), DispatcherPriority.ContextIdle);
                        Application.Current.Dispatcher.Invoke(() => Temperature6Contexts.Insert(0, new BarContext(DateTime.Now, Temperature6)), DispatcherPriority.ContextIdle);
                        break;
                    case Bms.TelemetryId.Temperature7:
                        Temperature7 = double.Parse(message.Value.ToString());
                        if (Temperature7Contexts.Count >= MAX_GRAPH_SIZE)
                            Application.Current.Dispatcher.Invoke(() => Temperature7Contexts.RemoveAt(Temperature7Contexts.Count - 1), DispatcherPriority.ContextIdle);
                        Application.Current.Dispatcher.Invoke(() => Temperature7Contexts.Insert(0, new BarContext(DateTime.Now, Temperature7)), DispatcherPriority.ContextIdle);
                        break;
                    case Bms.TelemetryId.Voltage1:
                        Voltage1 = double.Parse(message.Value.ToString());
                        if (Voltage1Contexts.Count >= MAX_GRAPH_SIZE)
                            Application.Current.Dispatcher.Invoke(() => Voltage1Contexts.RemoveAt(Voltage1Contexts.Count - 1), DispatcherPriority.ContextIdle);
                        Application.Current.Dispatcher.Invoke(() => Voltage1Contexts.Insert(0, new BarContext(DateTime.Now, Voltage1)), DispatcherPriority.ContextIdle);
                        break;
                    case Bms.TelemetryId.Voltage2:
                        Voltage2 = double.Parse(message.Value.ToString());
                        if (Voltage2Contexts.Count >= MAX_GRAPH_SIZE)
                            Application.Current.Dispatcher.Invoke(() => Voltage2Contexts.RemoveAt(Voltage2Contexts.Count - 1), DispatcherPriority.ContextIdle);
                        Application.Current.Dispatcher.Invoke(() => Voltage2Contexts.Insert(0, new BarContext(DateTime.Now, Voltage2)), DispatcherPriority.ContextIdle);
                        break;
                    case Bms.TelemetryId.Voltage3:
                        Voltage3 = double.Parse(message.Value.ToString());
                        if (Voltage3Contexts.Count >= MAX_GRAPH_SIZE)
                            Application.Current.Dispatcher.Invoke(() => Voltage3Contexts.RemoveAt(Voltage3Contexts.Count - 1), DispatcherPriority.ContextIdle);
                        Application.Current.Dispatcher.Invoke(() => Voltage3Contexts.Insert(0, new BarContext(DateTime.Now, Voltage3)), DispatcherPriority.ContextIdle);
                        break;
                    case Bms.TelemetryId.Voltage4:
                        Voltage4 = double.Parse(message.Value.ToString());
                        if (Voltage4Contexts.Count >= MAX_GRAPH_SIZE)
                            Application.Current.Dispatcher.Invoke(() => Voltage4Contexts.RemoveAt(Voltage4Contexts.Count - 1), DispatcherPriority.ContextIdle);
                        Application.Current.Dispatcher.Invoke(() => Voltage4Contexts.Insert(0, new BarContext(DateTime.Now, Voltage4)), DispatcherPriority.ContextIdle);
                        break;
                    case Bms.TelemetryId.Voltage5:
                        Voltage5 = double.Parse(message.Value.ToString());
                        if (Voltage5Contexts.Count >= MAX_GRAPH_SIZE)
                            Application.Current.Dispatcher.Invoke(() => Voltage5Contexts.RemoveAt(Voltage5Contexts.Count - 1), DispatcherPriority.ContextIdle);
                        Application.Current.Dispatcher.Invoke(() => Voltage5Contexts.Insert(0, new BarContext(DateTime.Now, Voltage5)), DispatcherPriority.ContextIdle);
                        break;
                    case Bms.TelemetryId.Voltage6:
                        Voltage6 = double.Parse(message.Value.ToString());
                        if (Voltage6Contexts.Count >= MAX_GRAPH_SIZE)
                            Application.Current.Dispatcher.Invoke(() => Voltage6Contexts.RemoveAt(Voltage6Contexts.Count - 1), DispatcherPriority.ContextIdle);
                        Application.Current.Dispatcher.Invoke(() => Voltage6Contexts.Insert(0, new BarContext(DateTime.Now, Voltage6)), DispatcherPriority.ContextIdle);
                        break;
                    case Bms.TelemetryId.Voltage7:
                        Voltage7 = double.Parse(message.Value.ToString());
                        if (Voltage7Contexts.Count >= MAX_GRAPH_SIZE)
                            Application.Current.Dispatcher.Invoke(() => Voltage7Contexts.RemoveAt(Voltage7Contexts.Count - 1), DispatcherPriority.ContextIdle);
                        Application.Current.Dispatcher.Invoke(() => Voltage7Contexts.Insert(0, new BarContext(DateTime.Now, Voltage7)), DispatcherPriority.ContextIdle);
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
        
        #region Mathematical Values

        private const double BatteryCapacity = 21.88;
        private const double PeukertPower = 1.01537;

        private const double MainBatteryCurrentOffset = 512;
        private const double MainBatteryCurrentConversionValue = 10.23;
        private double mainBatteryMinimumCurrent = -15;
        public double MainBatteryMinimumCurrent
        {
            get { return mainBatteryMinimumCurrent; }
            set { SetField(ref mainBatteryMinimumCurrent, value); }
        }
        private double mainBatteryMaximumCurrent = 52;
        public double MainBatteryMaximumCurrent
        {
            get { return mainBatteryMaximumCurrent; }
            set { SetField(ref mainBatteryMaximumCurrent, value); }
        }

        private const double MainBatteryVoltageConversionValue = 34.15692;
        private double minimumMainBatteryVoltage = 18.9;
        public double MinimumMainBatteryVoltage
        {
            get { return minimumMainBatteryVoltage; }
            set { SetField(ref minimumMainBatteryVoltage, value); }
        }
        private double maximumMainBatteryVoltage = 29.4;
        public double MaximumMainBatteryVoltage
        {
            get { return maximumMainBatteryVoltage; }
            set { SetField(ref maximumMainBatteryVoltage, value); }
        }

        private const double TemperatureConversionValue = 0.48875;
        private double minimumTemperature; // 0m by default
        public double MinimumTemperature
        {
            get { return minimumTemperature; }
            set { SetField(ref minimumTemperature, value); }
        }
        private double maximumTemperature = 60;
        public double MaximumTemperature
        {
            get { return maximumTemperature; }
            set { SetField(ref maximumTemperature, value); }
        }

        private const double VoltageConversionValue = 0.009775;
        private double minimumVoltage = 2.7;
        public double MinimumVoltage
        {
            get { return minimumVoltage; }
            set { SetField(ref minimumVoltage, value); }
        }
        private double maximumVoltage = 4.2;
        public double MaximumVoltage
        {
            get { return maximumVoltage; }
            set { SetField(ref maximumVoltage, value); }
        }

        #endregion
    }
}