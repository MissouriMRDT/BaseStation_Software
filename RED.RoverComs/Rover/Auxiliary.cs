namespace RED.RoverComs.Rover
{
    // Id Range: 6000 - 6999
    public class Auxiliary
    {
        public enum TelemetryId
        {
            DrillHydrogenReading = 6015,
            DrillMethaneReading = 6016,
            DrillAmmoniaReading = 6017,
            DrillTemperature = 6018,
            DrillActualSpeed = 6019
        }
        public enum CommandId
        {
            DrillSpeed = 6001,
            DrillDirection = 6002,
            HeaterPower = 6003,
            ThermalReadings = 6004,
            SensorPower = 6005,
            GasReadings = 6006,
            Gripper = 6007,

            Light395 = 6008,
            Light440 = 6009,
            Door = 6010,
            ToggleDrillSensorTelemetry = 6011,
            RedLight = 6012,
            GreenLight = 6013,
            BlueLight = 6014
        }
    }
}
