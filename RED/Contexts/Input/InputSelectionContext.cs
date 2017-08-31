using System;
using System.Xml.Serialization;

namespace RED.Contexts.Input
{
    [XmlType(TypeName = "InputSelectionData")]
    public class InputSelectionsContext : ConfigurationFile
    {
        public InputSelectionContext[] Selections;

        public InputSelectionsContext()
        { }

        public InputSelectionsContext(InputSelectionContext[] selections)
            : this()
        {
            Selections = selections;
        }
    }

    [Serializable]
    [XmlType(TypeName = "Selection")]
    public class InputSelectionContext
    {
        public string ModeName;
        public string DeviceName;
        public string MappingName;
        public bool Active;

        private InputSelectionContext()
        { }

        public InputSelectionContext(string modeName, string deviceName, string mappingName, bool active)
        {
            ModeName = modeName;
            DeviceName = deviceName;
            MappingName = mappingName;
            Active = active;
        }
    }
}
