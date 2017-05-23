using System;
using System.Xml.Serialization;

namespace RED.Contexts
{
    [XmlType(TypeName = "Selection")]
    [Serializable]
    public class InputSelectionContext
    {
        public string ModeName;
        public string DeviceName;
        public string MappingName;
        public bool Active;

        public InputSelectionContext()
        { }
    }
}
