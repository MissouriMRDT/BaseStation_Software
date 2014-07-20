namespace RED.Models.Modules
{
    using Contexts;
    using System.Collections.ObjectModel;

    internal class BatteryModel
    {
        internal string Title = "Battery System";
        internal bool InUse = false;
        internal bool IsManageable = true;

        internal readonly ObservableCollection<BarContext> MainBatteryCurrentContexts = new ObservableCollection<BarContext>();
        internal readonly ObservableCollection<BarContext> MainBatteryVoltageContexts = new ObservableCollection<BarContext>();
        internal readonly ObservableCollection<BarContext> Temperature1Contexts = new ObservableCollection<BarContext>();
        internal readonly ObservableCollection<BarContext> Temperature2Contexts = new ObservableCollection<BarContext>();
        internal readonly ObservableCollection<BarContext> Temperature3Contexts = new ObservableCollection<BarContext>();
        internal readonly ObservableCollection<BarContext> Temperature4Contexts = new ObservableCollection<BarContext>();
        internal readonly ObservableCollection<BarContext> Temperature5Contexts = new ObservableCollection<BarContext>();
        internal readonly ObservableCollection<BarContext> Temperature6Contexts = new ObservableCollection<BarContext>();
        internal readonly ObservableCollection<BarContext> Temperature7Contexts = new ObservableCollection<BarContext>();
        internal readonly ObservableCollection<BarContext> Voltage1Contexts = new ObservableCollection<BarContext>();
        internal readonly ObservableCollection<BarContext> Voltage2Contexts = new ObservableCollection<BarContext>();
        internal readonly ObservableCollection<BarContext> Voltage3Contexts = new ObservableCollection<BarContext>();
        internal readonly ObservableCollection<BarContext> Voltage4Contexts = new ObservableCollection<BarContext>();
        internal readonly ObservableCollection<BarContext> Voltage5Contexts = new ObservableCollection<BarContext>();
        internal readonly ObservableCollection<BarContext> Voltage6Contexts = new ObservableCollection<BarContext>();
        internal readonly ObservableCollection<BarContext> Voltage7Contexts = new ObservableCollection<BarContext>();

        internal double MainBatteryCurrent;
        internal double MainBatteryVoltage;
        internal double Temperature1;
        internal double Temperature2;
        internal double Temperature3;
        internal double Temperature4;
        internal double Temperature5;
        internal double Temperature6;
        internal double Temperature7;
        internal double Voltage1;
        internal double Voltage2;
        internal double Voltage3;
        internal double Voltage4;
        internal double Voltage5;
        internal double Voltage6;
        internal double Voltage7;
    }
}
