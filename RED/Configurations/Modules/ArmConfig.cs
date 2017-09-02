using RED.Contexts.Modules;

namespace RED.Configurations.Modules
{
    internal static class ArmConfig
    {
        internal static ArmPositionsContext DefaultArmPositions = new ArmPositionsContext(new[] {
            new ArmPositionContext("Right Bay", 110.24f, 115.83f, 63.75f, 0, 180, 0),
            new ArmPositionContext("Left Bay", 255, 115.83f, 63.75f, 0, 180, 0),
            new ArmPositionContext("Reset Home", 0, 80, 100, 0, 185, 0)
        });
    }
}
