namespace RED.Models.Input
{
    internal class MappingChannelModel
    {
        internal string InputKey;
        internal string OutputKey;
        internal float LinearScaling = 1F;
        internal bool Parabolic = false;
        internal float Minimum = -1F;
        internal float Maximum = 1F;
        internal float Offset = 0F;
    }
}
