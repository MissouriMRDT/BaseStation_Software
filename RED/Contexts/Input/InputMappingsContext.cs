using System.Xml.Serialization;

namespace RED.Contexts.Input
{
    [XmlType(TypeName = "InputMappings")]
    public class InputMappingsContext : ConfigurationFile
    {
        public InputMappingContext[] Mappings;

        public InputMappingsContext()
        { }

        public InputMappingsContext(InputMappingContext[] mappings)
            : this()
        {
            Mappings = mappings;
        }
    }

    [XmlType(TypeName = "Mapping")]
    public class InputMappingContext : ConfigurationFile
    {
        public string Name;
        public InputChannelContext[] Channels;
        public string DeviceType;
        public string ModeType;
        public int UpdatePeriod;

        public InputMappingContext()
        { }

        public InputMappingContext(string name, string deviceType, string modeType, int updatePeriod, InputChannelContext[] channels)
            : this()
        {
            Name = name;
            DeviceType = deviceType;
            ModeType = modeType;
            UpdatePeriod = updatePeriod;
            Channels = channels;
        }
    }

    [XmlType(TypeName = "Channel")]
    public class InputChannelContext : ConfigurationFile
    {
        public string InputKey;
        public string OutputKey;
        public float LinearScaling = 1F;
        public bool Parabolic = false;
        public float Minimum = -1F;
        public float Maximum = 1F;
        public float Offset = 0F;

        public InputChannelContext()
        { }

        public InputChannelContext(string inputKey, string outputKey)
            : this()
        {
            InputKey = inputKey;
            OutputKey = outputKey;
        }

        public InputChannelContext(string inputKey, string outputKey, float linearScaling, bool parabolic, float min, float max, float offest)
            : this(inputKey, outputKey)
        {
            LinearScaling = linearScaling;
            Parabolic = parabolic;
            Minimum = min;
            Maximum = max;
            Offset = offest;
        }
    }
}
