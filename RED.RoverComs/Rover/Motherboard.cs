namespace RED.RoverComs.Rover
{
    // Id Range: 1000 - 1999
    public class Motherboard
    {
        public enum TelemetryId
        {
            GpsFix = 1001,
            GpsLatitude = 1002,
            GpsLongitude = 1003,
            GpsAltitude = 1004,
            GpsSatelliteCount = 1005
        }
        public enum CommandId
        {
            ToggleGpsTelemetry = 1006
        }
    }
}
