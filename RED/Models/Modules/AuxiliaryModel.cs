using RED.Contexts;
namespace RED.Models.Modules
{
    internal class AuxiliaryModel
    {
        internal string Title = "Auxiliary Systems";
        internal bool InUse = false;
        internal bool IsManageable = true;

        internal TelemetryContext<bool> GpsFixed;
        internal TelemetryContext<string> GpsLatitude;
        internal TelemetryContext<string> GpsLongitude;
        internal TelemetryContext<string> GpsAltitude;
        internal TelemetryContext<int> GpsSatelliteCount;

        internal int CurrentDrillSpeed = 0;

        internal TelemetryContext<int> DrillHydrogenReading;
        internal TelemetryContext<int> DrillMethaneReading;
        internal TelemetryContext<int> DrillAmmoniaReading;
        internal TelemetryContext<double> DrillTemperature;
        internal TelemetryContext<int> DrillActualSpeed;
    }
}
