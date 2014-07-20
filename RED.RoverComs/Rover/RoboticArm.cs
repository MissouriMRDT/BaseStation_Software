namespace RED.RoverComs.Rover
{
    // Id Range: 2000 - 2999
    public static class RoboticArm
    {
        public enum TelemetryId
        {
            
        }
        public enum CommandId
        {
            Wrist = 2001,
            Elbow = 2002,
            Actuator = 2003,
            Base = 2004,
            Reset = 2005
        }
    }
}
